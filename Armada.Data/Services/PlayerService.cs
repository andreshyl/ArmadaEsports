using ArmadaEsports.Core.Interfaces;
using ArmadaEsports.Core.Models;
using ArmadaEsports.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace ArmadaEsports.Data.Services;

public class PlayerService(ArmadaDbContext db) : IPlayerService
{
    public async Task<List<Player>> GetAllAsync(bool includeInactive = false)
    {
        var query = db.Players
            .Include(p => p.PrimaryPosition)
            .Include(p => p.SecondaryPosition)
            .Include(p => p.TertiaryPosition)
            .Include(p => p.AttributeSnapshots)
                .ThenInclude(s => s.Scores)
            .Include(p => p.PerformanceStats)
                .ThenInclude(s => s.Match)
            .AsQueryable();

        if (!includeInactive)
            query = query.Where(p => p.IsActive);

        return await query
            .OrderBy(p => p.JerseyNumber == null)
            .ThenBy(p => p.JerseyNumber)
            .ThenBy(p => p.Alias)
            .ToListAsync();
    }

    public async Task<List<Position>> GetAllPositionsAsync() =>
        await db.Positions.OrderBy(p => p.SortOrder).ToListAsync();

    public async Task<Player?> GetByIdAsync(int id) =>
        await db.Players
            .Include(p => p.PrimaryPosition)
            .Include(p => p.SecondaryPosition)
            .Include(p => p.TertiaryPosition)
            .Include(p => p.AttributeSnapshots)
                .ThenInclude(s => s.Scores)
            .Include(p => p.PerformanceStats)
                .ThenInclude(s => s.Match)
            .FirstOrDefaultAsync(p => p.Id == id);

    public async Task<Player?> GetByAliasAsync(string alias) =>
        await db.Players
            .Include(p => p.PrimaryPosition)
            .FirstOrDefaultAsync(p => p.Alias == alias);

    public async Task<Player> CreateAsync(Player player)
    {
        db.Players.Add(player);
        await db.SaveChangesAsync();
        return player;
    }

    public async Task<Player> UpdateAsync(Player player)
    {
        db.Players.Update(player);
        await db.SaveChangesAsync();
        return player;
    }

    public async Task<bool> DeactivateAsync(int id)
    {
        var player = await db.Players.FindAsync(id);
        if (player is null) return false;
        player.IsActive = false;
        await db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> AliasExistsAsync(string alias, int? excludeId = null) =>
        await db.Players.AnyAsync(p => p.Alias == alias && p.Id != excludeId);

    // null jersey is always allowed — only non-null values are checked for uniqueness
    public async Task<bool> JerseyNumberExistsAsync(int jerseyNumber, int? excludeId = null) =>
        await db.Players.AnyAsync(p => p.JerseyNumber == jerseyNumber && p.Id != excludeId);
}
