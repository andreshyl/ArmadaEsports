using ArmadaEsports.Core.Models;

namespace ArmadaEsports.Core.Interfaces;

public interface IPlayerService
{
    Task<List<Player>> GetAllAsync(bool includeInactive = false);
    Task<List<Position>> GetAllPositionsAsync();
    Task<Player?> GetByIdAsync(int id);
    Task<Player?> GetByAliasAsync(string alias);
    Task<Player> CreateAsync(Player player);
    Task<Player> UpdateAsync(Player player);
    Task<bool> DeactivateAsync(int id);
    Task<bool> AliasExistsAsync(string alias, int? excludeId = null);
    Task<bool> JerseyNumberExistsAsync(int jerseyNumber, int? excludeId = null);
}
