using ArmadaEsports.Core.Enums;
using ArmadaEsports.Core.Models.Base;

namespace ArmadaEsports.Core.Models;

public class Match : BaseAuditableEntity
{
    public int CompetitionId { get; set; }
    public DateTime MatchDate { get; set; }
    public short? Matchday { get; set; }
    public string? MatchdayLabel { get; set; }
    public string OpponentName { get; set; } = string.Empty;
    public EVenue Venue { get; set; } = EVenue.Home;
    public byte? GoalsFor { get; set; }
    public byte? GoalsAgainst { get; set; }
    public EMatchResult? Result { get; set; }
    public EMatchStatus Status { get; set; } = EMatchStatus.Upcoming;

    // Team stats from Match Facts screen
    public byte? PossessionPct { get; set; }
    public byte? ShotsFor { get; set; }
    public byte? ShotsAgainst { get; set; }
    public short? PassesAttempted { get; set; }
    public byte? PassAccuracyPct { get; set; }
    public byte? TacklesFor { get; set; }
    public byte? TacklesAgainst { get; set; }

    public Competition Competition { get; set; } = null!;
    public ICollection<MatchPerformanceStat> PerformanceStats { get; set; } = [];
    public ICollection<AiMatchParseJob> ParseJobs { get; set; } = [];
}
