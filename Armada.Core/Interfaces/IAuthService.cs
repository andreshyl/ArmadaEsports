using ArmadaEsports.Core.Models;

namespace ArmadaEsports.Core.Interfaces;

public interface IAuthService
{
    Task<User?> LoginAsync(string username, string password);
    Task<(bool Success, string? Error)> RegisterAsync(string username, string email, string password);
    Task<User?> GetByIdAsync(int id);
    Task<bool> AnyUsersAsync();
}
