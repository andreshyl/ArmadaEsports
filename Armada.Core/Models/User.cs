using ArmadaEsports.Core.Models.Base;

namespace ArmadaEsports.Core.Models;

public class User : BaseAuditableEntity
{
    public string Username { get; set; } = string.Empty;
    public string Email    { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Role     { get; set; } = "Manager";
    public bool   IsActive { get; set; } = true;
}
