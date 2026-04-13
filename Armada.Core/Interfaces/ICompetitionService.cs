using ArmadaEsports.Core.Models;

namespace ArmadaEsports.Core.Interfaces;

public interface ICompetitionService
{
    Task<List<Competition>> GetAllAsync(bool includeInactive = false);
    Task<Competition?> GetByIdAsync(int id);
    Task<Competition> CreateAsync(Competition competition);
    Task<Competition> UpdateAsync(Competition competition);
    Task DeleteAsync(int competitionId);
}
