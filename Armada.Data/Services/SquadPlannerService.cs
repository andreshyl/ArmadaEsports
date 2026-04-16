using System.Text.Json;
using ArmadaEsports.Core.Interfaces;
using ArmadaEsports.Core.Models;
using Microsoft.Extensions.Configuration;

namespace ArmadaEsports.Data.Services;

public class SquadPlannerService(IConfiguration config) : ISquadPlannerService
{
    private readonly string _dir = Path.Combine(
        config["LineupsPath"] ?? Path.Combine(AppContext.BaseDirectory, "Data", "Lineups"));
    private readonly JsonSerializerOptions _opts = new() { WriteIndented = true };

    private string FilePath(string id) => Path.Combine(_dir, $"{id}.json");
    private void EnsureDir() => Directory.CreateDirectory(_dir);

    public async Task<List<LineupSnapshot>> GetAllAsync()
    {
        EnsureDir();
        var result = new List<LineupSnapshot>();
        foreach (var file in Directory.GetFiles(_dir, "*.json"))
        {
            try
            {
                var snap = JsonSerializer.Deserialize<LineupSnapshot>(await File.ReadAllTextAsync(file), _opts);
                if (snap is not null) result.Add(snap);
            }
            catch { }
        }
        return [.. result.OrderByDescending(s => s.Date)];
    }

    public async Task<LineupSnapshot?> GetByIdAsync(string id)
    {
        var path = FilePath(id);
        if (!File.Exists(path)) return null;
        return JsonSerializer.Deserialize<LineupSnapshot>(await File.ReadAllTextAsync(path), _opts);
    }

    public async Task SaveAsync(LineupSnapshot snapshot)
    {
        EnsureDir();
        await File.WriteAllTextAsync(FilePath(snapshot.Id), JsonSerializer.Serialize(snapshot, _opts));
    }

    public async Task DeleteAsync(string id)
    {
        var path = FilePath(id);
        if (File.Exists(path)) await Task.Run(() => File.Delete(path));
    }
}
