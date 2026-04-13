using ArmadaEsports.Core.Models;

namespace ArmadaEsports.Core.Interfaces;

public interface IAttributeService
{
    Task<List<PlayerAttributeSnapshot>> GetSnapshotsByPlayerAsync(int playerId);
    Task<PlayerAttributeSnapshot?> GetLatestSnapshotAsync(int playerId);
    Task<PlayerAttributeSnapshot> SaveSnapshotAsync(int playerId, string positionCode, byte[] scores, string? notes);
    Task DeleteSnapshotAsync(int snapshotId);
    decimal ComputeOverallRating(string positionCode, byte[] scores);
    decimal[] ComputeAverageScores(IEnumerable<PlayerAttributeSnapshot> snapshots);
    decimal ComputeAverageOverall(IEnumerable<PlayerAttributeSnapshot> snapshots);
}
