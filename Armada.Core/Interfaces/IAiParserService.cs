using ArmadaEsports.Core.Models;

namespace ArmadaEsports.Core.Interfaces;

public interface IAiParserService
{
    Task<AiMatchParseJob> SubmitImageAsync(int matchId, string imageBase64, string mimeType);
    Task<AiMatchParseJob?> GetJobAsync(Guid jobId);
    Task ConfirmJobAsync(Guid jobId, List<AiParseJobRow> editedRows);
    Task<MatchFactsResult> ExtractMatchFactsAsync(string imageBase64, string mimeType, string ttfSide = "right");
}

public class MatchFactsResult
{
    public byte? PossessionPct { get; set; }
    public byte? ShotsFor { get; set; }
    public byte? ShotsAgainst { get; set; }
    public short? PassesAttempted { get; set; }
    public byte? PassAccuracyPct { get; set; }
    public byte? TacklesFor { get; set; }
    public byte? TacklesAgainst { get; set; }
}
