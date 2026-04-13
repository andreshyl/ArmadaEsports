using ArmadaEsports.Core.Models;

namespace ArmadaEsports.Core.Interfaces;

public interface IMatchService
{
    Task<List<Match>> GetAllAsync();
    Task<List<Match>> GetUpcomingAsync();
    Task<List<Match>> GetFinishedAsync();
    Task<List<Match>> GetByCompetitionAsync(int competitionId);
    Task<Match?> GetByIdAsync(int id);
    Task<Match> CreateAsync(Match match);
    Task FinalizeAsync(int matchId, byte goalsFor, byte goalsAgainst);
    Task AddPerformanceStatAsync(MatchPerformanceStat stat);
    Task DeleteAsync(int matchId);
    Task UpdateMatchFactsAsync(int matchId, byte? possession, byte? shotsFor, byte? shotsAgainst,
        short? passesAttempted, byte? passAccuracy, byte? tacklesFor, byte? tacklesAgainst);
}
