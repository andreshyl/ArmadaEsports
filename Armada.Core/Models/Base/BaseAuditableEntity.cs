namespace ArmadaEsports.Core.Models.Base;

public abstract class BaseAuditableEntity : BaseEntity
{
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
