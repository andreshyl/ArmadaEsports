using ArmadaEsports.Core.Interfaces;
using ArmadaEsports.Core.Models;
using ArmadaEsports.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace ArmadaEsports.Data.Services;

public class CompetitionService(ArmadaDbContext db) : ICompetitionService
{
    public async Task<List<Competition>> GetAllAsync(bool includeInactive = false)
    {
        var query = db.Competitions.Include(c => c.Matches).AsQueryable();
        if (!includeInactive) query = query.Where(c => c.IsActive);
        return await query.OrderBy(c => c.Name).ToListAsync();
    }

    public async Task<Competition?> GetByIdAsync(int id) =>
        await db.Competitions.Include(c => c.Matches).FirstOrDefaultAsync(c => c.Id == id);

    public async Task<Competition> CreateAsync(Competition competition)
    {
        db.Competitions.Add(competition);
        await db.SaveChangesAsync();
        return competition;
    }

    public async Task<Competition> UpdateAsync(Competition competition)
    {
        db.Competitions.Update(competition);
        await db.SaveChangesAsync();
        return competition;
    }

    public async Task SetActiveAsync(int id, bool isActive)
    {
        var c = await db.Competitions.FindAsync(id)
            ?? throw new KeyNotFoundException($"Competition {id} not found.");
        c.IsActive = isActive;
        await db.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var c = await db.Competitions
            .Include(c => c.Matches)
                .ThenInclude(m => m.PerformanceStats)
            .Include(c => c.Matches)
                .ThenInclude(m => m.ParseJobs)
                    .ThenInclude(j => j.Rows)
            .FirstOrDefaultAsync(c => c.Id == id);
        if (c is null) return;
        db.Competitions.Remove(c);
        await db.SaveChangesAsync();
    }
}
