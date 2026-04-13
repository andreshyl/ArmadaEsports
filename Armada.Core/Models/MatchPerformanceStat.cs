using ArmadaEsports.Core.Models.Base;

namespace ArmadaEsports.Core.Models;

public class MatchPerformanceStat : BaseEntity
{
    public int MatchId { get; set; }
    public int PlayerId { get; set; }

    public byte Goals { get; set; } = 0;
    public decimal? ShotAccuracyPct { get; set; }
    public byte Assists { get; set; } = 0;
    public decimal? PassAccuracyPct { get; set; }
    public byte Tackles { get; set; } = 0;
    public byte TacklesWon { get; set; } = 0;
    public byte Interceptions { get; set; } = 0;
    public byte Saves { get; set; } = 0;
    public decimal? MatchRating { get; set; }

    public decimal? AiConfidenceScore { get; set; }
    public bool IsAiParsed { get; set; } = false;
    public DateTime? ConfirmedAt { get; set; }

    public Match Match { get; set; } = null!;
    public Player Player { get; set; } = null!;
}
