using ArmadaEsports.Core.Interfaces;
using ArmadaEsports.Core.Models;
using ArmadaEsports.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace ArmadaEsports.Data.Services;

public class CompetitionService(IDbContextFactory<ArmadaDbContext> factory) : ICompetitionService
{
    public async Task<List<Competition>> GetAllAsync(bool includeInactive = false)
    {
        await using var db = await factory.CreateDbContextAsync();
        var query = db.Competitions.Include(c => c.Matches).AsQueryable();
        if (!includeInactive) query = query.Where(c => c.IsActive);
        return await query.OrderBy(c => c.Name).ToListAsync();
    }

    public async Task<Competition?> GetByIdAsync(int id)
    {
        await using var db = await factory.CreateDbContextAsync();
        return await db.Competitions.Include(c => c.Matches).FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Competition> CreateAsync(Competition competition)
    {
        await using var db = await factory.CreateDbContextAsync();
        db.Competitions.Add(competition);
        await db.SaveChangesAsync();
        return competition;
    }

    public async Task<Competition> UpdateAsync(Competition competition)
    {
        await using var db = await factory.CreateDbContextAsync();
        db.Competitions.Update(competition);
        await db.SaveChangesAsync();
        return competition;
    }

    public async Task DeleteAsync(int competitionId)
    {
        await using var db = await factory.CreateDbContextAsync();
        var competition = await db.Competitions.FindAsync(competitionId)
            ?? throw new KeyNotFoundException($"Competition {competitionId} not found.");
        db.Competitions.Remove(competition);
        await db.SaveChangesAsync();
    }
}
