using ArmadaEsports.Core.Models;

namespace ArmadaEsports.Core.Interfaces;

public interface ISquadPlannerService
{
    Task<List<LineupSnapshot>> GetAllAsync();
    Task<LineupSnapshot?> GetByIdAsync(string id);
    Task SaveAsync(LineupSnapshot snapshot);
    Task DeleteAsync(string id);
}
