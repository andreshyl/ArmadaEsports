using ArmadaEsports.Core.Models.Base;

namespace ArmadaEsports.Core.Models;

public class PlayerAttributeSnapshot : BaseEntity
{
    public int PlayerId { get; set; }
    public DateOnly SnapshotDate { get; set; }
    public decimal OverallRating { get; set; }
    public string? Notes { get; set; }

    public Player Player { get; set; } = null!;
    public ICollection<PlayerAttributeScore> Scores { get; set; } = [];
}
