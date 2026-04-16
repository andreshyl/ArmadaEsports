using BCrypt.Net;
using ArmadaEsports.Core.Interfaces;
using ArmadaEsports.Core.Models;
using ArmadaEsports.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace ArmadaEsports.Data.Services;

public class AuthService(IDbContextFactory<ArmadaDbContext> factory) : IAuthService
{
    public async Task<User?> LoginAsync(string username, string password)
    {
        await using var db = await factory.CreateDbContextAsync();
        var user = await db.Users.FirstOrDefaultAsync(u => u.Username == username && u.IsActive);
        if (user is null) return null;
        return BCrypt.Net.BCrypt.Verify(password, user.PasswordHash) ? user : null;
    }

    public async Task<(bool Success, string? Error)> RegisterAsync(string username, string email, string password)
    {
        await using var db = await factory.CreateDbContextAsync();
        if (await db.Users.AnyAsync(u => u.Username == username))
            return (false, "Username already taken.");
        if (await db.Users.AnyAsync(u => u.Email == email))
            return (false, "Email already registered.");

        db.Users.Add(new User
        {
            Username = username.Trim(),
            Email = email.Trim().ToLower(),
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            Role = "Manager",
        });
        await db.SaveChangesAsync();
        return (true, null);
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        await using var db = await factory.CreateDbContextAsync();
        return await db.Users.FindAsync(id);
    }

    public async Task<bool> AnyUsersAsync()
    {
        await using var db = await factory.CreateDbContextAsync();
        return await db.Users.AnyAsync();
    }
}
