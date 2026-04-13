using ArmadaEsports.Core.Enums;
using ArmadaEsports.Core.Interfaces;
using ArmadaEsports.Core.Models;
using ArmadaEsports.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Mscc.GenerativeAI;
using Mscc.GenerativeAI.Types;
using System.Text.Json;

namespace ArmadaEsports.Data.Services;

public class AiParserService(ArmadaDbContext db, IConfiguration config) : IAiParserService
{
    private const string Prompt = """
        You are extracting EA FC 26 Pro Clubs post-match stats from a screenshot.
        The screen shows a Player Performance table with columns: POS, Name, MR (match rating), G (goals), AST (assists).
        There may also be detailed stats on the right side for the selected player.
        Extract ALL players visible in the table on the left side.
        Return ONLY a valid JSON array. Each element must have these exact keys:
          alias        - player name/gamertag from the Name column
          goals        - integer from G column (0 if not shown)
          assists      - integer from AST column (0 if not shown)
          match_rating - decimal from MR column (e.g. 6.4)
          shot_accuracy_pct  - decimal percentage if visible on right panel, else null
          pass_accuracy_pct  - decimal percentage if visible on right panel, else null
          tackles      - integer if visible, else null
          tackles_won  - integer if visible, else null
          interceptions - integer if visible, else null
          saves        - integer if visible, else null
          confidence   - decimal 0.0-1.0 indicating how clearly you could read each row
        Return ONLY the JSON array. No markdown. No explanation. No code fences.
        """;

    private GenerativeModel GetModel()
    {
        var apiKey = config["Gemini:ApiKey"]
            ?? throw new InvalidOperationException("Gemini:ApiKey not configured.");
        var google = new GoogleAI(apiKey);
        return google.GenerativeModel(model: Model.Gemini20Flash);
    }

    public async Task<AiMatchParseJob> SubmitImageAsync(int matchId, string imageBase64, string mimeType)
    {
        var job = new AiMatchParseJob
        {
            MatchId = matchId,
            ImageBase64 = imageBase64,
            ImageMimeType = mimeType,
            ParseStatus = EAiParseStatus.Pending
        };
        db.AiMatchParseJobs.Add(job);
        await db.SaveChangesAsync(); // get valid Id before MapToRow

        try
        {
            var model = GetModel();
            var content = new Content
            {
                Role = "user",
                Parts =
                [
                    new Part { InlineData = new InlineData { MimeType = mimeType, Data = imageBase64 } },
                    new Part { Text = Prompt }
                ]
            };
            var request = new GenerateContentRequest { Contents = [content] };
            var response = await model.GenerateContent(request);
            var raw = response.Text?.Trim() ?? "[]";

            // Strip markdown fences if returned despite instructions
            if (raw.StartsWith("```"))
                raw = raw[(raw.IndexOf('\n') + 1)..].Replace("```", "").Trim();

            var parsed = JsonSerializer.Deserialize<List<AiRowDto>>(raw,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? [];

            job.RawOutput = raw;
            job.ParseStatus = EAiParseStatus.Parsed;
            job.Rows = parsed.Select(r => MapToRow(job.Id, r)).ToList();
        }
        catch (Exception ex)
        {
            job.ParseStatus = EAiParseStatus.Failed;
            job.ErrorMessage = ex.Message;
        }

        await db.SaveChangesAsync();
        return job;
    }

    public async Task<AiMatchParseJob?> GetJobAsync(Guid jobId) =>
        await db.AiMatchParseJobs
            .Include(j => j.Rows).ThenInclude(r => r.Player)
            .FirstOrDefaultAsync(j => j.Id == jobId);

    public async Task ConfirmJobAsync(Guid jobId, List<AiParseJobRow> editedRows)
    {
        var job = await db.AiMatchParseJobs.FindAsync(jobId)
            ?? throw new KeyNotFoundException($"Job {jobId} not found.");

        var stats = editedRows
            .Where(r => r.PlayerId.HasValue)
            .Select(r => new MatchPerformanceStat
            {
                MatchId = job.MatchId,
                PlayerId = r.PlayerId!.Value,
                Goals = r.Goals ?? 0,
                ShotAccuracyPct = r.ShotAccuracyPct,
                Assists = r.Assists ?? 0,
                PassAccuracyPct = r.PassAccuracyPct,
                Tackles = r.Tackles ?? 0,
                TacklesWon = r.TacklesWon ?? 0,
                Interceptions = r.Interceptions ?? 0,
                Saves = r.Saves ?? 0,
                MatchRating = r.MatchRating,
                AiConfidenceScore = r.AiConfidenceScore,
                IsAiParsed = true,
                ConfirmedAt = DateTime.UtcNow
            }).ToList();

        db.MatchPerformanceStats.AddRange(stats);
        job.ParseStatus = EAiParseStatus.Confirmed;
        job.ConfirmedAt = DateTime.UtcNow;
        await db.SaveChangesAsync();
    }

    private static AiParseJobRow MapToRow(Guid jobId, AiRowDto r) => new()
    {
        JobId = jobId,
        RawAlias = r.Alias,
        Goals = r.Goals,
        ShotAccuracyPct = r.ShotAccuracyPct,
        Assists = r.Assists,
        PassAccuracyPct = r.PassAccuracyPct,
        Tackles = r.Tackles,
        TacklesWon = r.TacklesWon,
        Interceptions = r.Interceptions,
        Saves = r.Saves,
        MatchRating = r.MatchRating,
        AiConfidenceScore = r.Confidence
    };

    private sealed class AiRowDto
    {
        public string? Alias { get; set; }
        public byte? Goals { get; set; }
        public decimal? ShotAccuracyPct { get; set; }
        public byte? Assists { get; set; }
        public decimal? PassAccuracyPct { get; set; }
        public byte? Tackles { get; set; }
        public byte? TacklesWon { get; set; }
        public byte? Interceptions { get; set; }
        public byte? Saves { get; set; }
        public decimal? MatchRating { get; set; }
        public decimal Confidence { get; set; }
    }
    public async Task<MatchFactsResult> ExtractMatchFactsAsync(string imageBase64, string mimeType)
    {
        const string factsPrompt = """
            You are extracting EA FC 26 Match Facts stats from a screenshot.
            The screen shows team stats: Possession %, Shots, Passes Attempted, Pass Accuracy %, Tackles.
            The left column is AES (Armada Esports / our team), right column is the opponent.
            Return ONLY a valid JSON object with these exact keys:
              possession_pct    - our possession percentage (integer)
              shots_for         - our shots (integer)
              shots_against     - opponent shots (integer)
              passes_attempted  - our passes attempted (integer)
              pass_accuracy_pct - our pass accuracy percentage (integer)
              tackles_for       - our tackles (integer)
              tackles_against   - opponent tackles (integer)
            Return ONLY the JSON object. No markdown. No explanation.
            """;

        var model = GetModel();
        var contentObj = new Content
        {
            Role = "user",
            Parts =
            [
                new Part { InlineData = new InlineData { MimeType = mimeType, Data = imageBase64 } },
                new Part { Text = factsPrompt }
            ]
        };
        var request = new GenerateContentRequest { Contents = [contentObj] };
        var response = await model.GenerateContent(request);
        var raw = response.Text?.Trim() ?? "{}";

        if (raw.StartsWith("```"))
            raw = raw[(raw.IndexOf('\n') + 1)..].Replace("```", "").Trim();

        using var doc = System.Text.Json.JsonDocument.Parse(raw);
        var root = doc.RootElement;

        static byte? GetByte(System.Text.Json.JsonElement r, string key) =>
            r.TryGetProperty(key, out var v) && v.ValueKind == System.Text.Json.JsonValueKind.Number
                ? (byte?)v.GetByte() : null;

        static short? GetShort(System.Text.Json.JsonElement r, string key) =>
            r.TryGetProperty(key, out var v) && v.ValueKind == System.Text.Json.JsonValueKind.Number
                ? (short?)v.GetInt16() : null;

        return new MatchFactsResult
        {
            PossessionPct = GetByte(root, "possession_pct"),
            ShotsFor = GetByte(root, "shots_for"),
            ShotsAgainst = GetByte(root, "shots_against"),
            PassesAttempted = GetShort(root, "passes_attempted"),
            PassAccuracyPct = GetByte(root, "pass_accuracy_pct"),
            TacklesFor = GetByte(root, "tackles_for"),
            TacklesAgainst = GetByte(root, "tackles_against"),
        };
    }

}
