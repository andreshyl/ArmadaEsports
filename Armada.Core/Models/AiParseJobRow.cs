namespace ArmadaEsports.Core.Models;

public class AiParseJobRow
{
    public int Id { get; set; }
    public Guid JobId { get; set; }
    public int? PlayerId { get; set; }
    public string? RawAlias { get; set; }
    public byte? Goals { get; set; }
    public decimal? ShotAccuracyPct { get; set; }
    public byte? Assists { get; set; }
    public decimal? PassAccuracyPct { get; set; }
    public byte? Tackles { get; set; }
    public byte? TacklesWon { get; set; }
    public byte? Interceptions { get; set; }
    public byte? Saves { get; set; }
    public decimal? MatchRating { get; set; }
    public decimal AiConfidenceScore { get; set; }
    public bool IsManuallyEdited { get; set; } = false;

    public AiMatchParseJob Job { get; set; } = null!;
    public Player? Player { get; set; }
}
