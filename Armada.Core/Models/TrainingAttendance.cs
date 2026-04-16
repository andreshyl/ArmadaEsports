using ArmadaEsports.Core.Models.Base;

namespace ArmadaEsports.Core.Models;

public class TrainingAttendance : BaseEntity
{
    public int PlayerId { get; set; }
    public DateOnly SessionDate { get; set; }
    public bool Present { get; set; }

    public Player Player { get; set; } = null!;
}
