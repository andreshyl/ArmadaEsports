using ArmadaEsports.Core.Models.Base;

namespace ArmadaEsports.Core.Models;

public class Player : BaseAuditableEntity
{
    public string Alias { get; set; } = string.Empty;
    public int JerseyNumber { get; set; }
    public int PrimaryPositionId { get; set; }
    public int? SecondaryPositionId { get; set; }
    public int? TertiaryPositionId { get; set; }
    public bool IsActive { get; set; } = true;
    public DateOnly? JoinedAt { get; set; }

    public Position PrimaryPosition { get; set; } = null!;
    public Position? SecondaryPosition { get; set; }
    public Position? TertiaryPosition { get; set; }
    public ICollection<PlayerAttributeSnapshot> AttributeSnapshots { get; set; } = [];
    public ICollection<MatchPerformanceStat> PerformanceStats { get; set; } = [];
}
