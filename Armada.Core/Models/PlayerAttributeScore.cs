namespace ArmadaEsports.Core.Models;

public class PlayerAttributeScore
{
    public int Id { get; set; }
    public int SnapshotId { get; set; }
    public byte AttributeIndex { get; set; }
    public byte Score { get; set; }

    public PlayerAttributeSnapshot Snapshot { get; set; } = null!;
}
