using ArmadaEsports.Core.Enums;
using ArmadaEsports.Core.Interfaces;
using ArmadaEsports.Core.Models;
using ArmadaEsports.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace ArmadaEsports.Data.Services;

public class MatchService(ArmadaDbContext db) : IMatchService
{
    private static IQueryable<Match> WithIncludes(IQueryable<Match> q) =>
        q.Include(m => m.Competition)
         .Include(m => m.PerformanceStats)
             .ThenInclude(s => s.Player)
                 .ThenInclude(p => p!.PrimaryPosition)
         .Include(m => m.ParseJobs);

    public async Task<List<Match>> GetAllAsync() =>
        await WithIncludes(db.Matches).OrderByDescending(m => m.MatchDate).ToListAsync();

    public async Task<List<Match>> GetByCompetitionAsync(int competitionId) =>
        await WithIncludes(db.Matches)
            .Where(m => m.CompetitionId == competitionId)
            .OrderByDescending(m => m.MatchDate).ToListAsync();

    public async Task<List<Match>> GetUpcomingAsync() =>
        await WithIncludes(db.Matches)
            .Where(m => m.Status == EMatchStatus.Upcoming)
            .OrderBy(m => m.MatchDate).ToListAsync();

    public async Task<List<Match>> GetFinishedAsync() =>
        await WithIncludes(db.Matches)
            .Where(m => m.Status == EMatchStatus.Finished)
            .OrderByDescending(m => m.MatchDate).ToListAsync();

    public async Task<Match?> GetByIdAsync(int id) =>
        await WithIncludes(db.Matches).FirstOrDefaultAsync(m => m.Id == id);

    public async Task<Match> CreateAsync(Match match)
    {
        db.Matches.Add(match);
        await db.SaveChangesAsync();
        return match;
    }

    public async Task FinalizeAsync(int id, byte goalsFor, byte goalsAgainst)
    {
        var match = await db.Matches.FindAsync(id)
            ?? throw new KeyNotFoundException($"Match {id} not found.");
        match.GoalsFor = goalsFor;
        match.GoalsAgainst = goalsAgainst;
        match.Status = EMatchStatus.Finished;
        match.Result = goalsFor > goalsAgainst ? EMatchResult.W
                           : goalsFor < goalsAgainst ? EMatchResult.L
                           : EMatchResult.D;
        await db.SaveChangesAsync();
    }

    public async Task AddPerformanceStatAsync(MatchPerformanceStat stat)
    {
        var existing = await db.MatchPerformanceStats
            .FirstOrDefaultAsync(s => s.MatchId == stat.MatchId && s.PlayerId == stat.PlayerId);
        if (existing is not null)
        {
            existing.Goals = stat.Goals;
            existing.Assists = stat.Assists;
            existing.PassAccuracyPct = stat.PassAccuracyPct;
            existing.ShotAccuracyPct = stat.ShotAccuracyPct;
            existing.Tackles = stat.Tackles;
            existing.TacklesWon = stat.TacklesWon;
            existing.Interceptions = stat.Interceptions;
            existing.Saves = stat.Saves;
            existing.MatchRating = stat.MatchRating;
            existing.ConfirmedAt = stat.ConfirmedAt;
        }
        else
        {
            db.MatchPerformanceStats.Add(stat);
        }
        await db.SaveChangesAsync();
    }

    public async Task DeleteAsync(int matchId)
    {
        var match = await db.Matches
            .Include(m => m.PerformanceStats)
            .Include(m => m.ParseJobs)
            .FirstOrDefaultAsync(m => m.Id == matchId);
        if (match is null) return;
        db.Matches.Remove(match);
        await db.SaveChangesAsync();
    }
    public async Task UpdateMatchFactsAsync(int matchId, byte? possession, byte? shotsFor,
        byte? shotsAgainst, short? passesAttempted, byte? passAccuracy,
        byte? tacklesFor, byte? tacklesAgainst,
        byte? possessionOpp = null, short? passesAttemptedOpp = null, byte? passAccuracyOpp = null)
    {
        var match = await db.Matches.FindAsync(matchId)
            ?? throw new KeyNotFoundException($"Match {matchId} not found.");
        match.PossessionPct = possession;
        match.ShotsFor = shotsFor;
        match.ShotsAgainst = shotsAgainst;
        match.PassesAttempted = passesAttempted;
        match.PassAccuracyPct = passAccuracy;
        match.TacklesFor = tacklesFor;
        match.TacklesAgainst = tacklesAgainst;
        match.PossessionPctOpp = possessionOpp;
        match.PassesAttemptedOpp = passesAttemptedOpp;
        match.PassAccuracyPctOpp = passAccuracyOpp;
        await db.SaveChangesAsync();
    }

}
