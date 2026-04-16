using ArmadaEsports.Core.Config;
using ArmadaEsports.Core.Interfaces;
using ArmadaEsports.Core.Models;
using ArmadaEsports.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace ArmadaEsports.Data.Services;

public class AttributeService(IDbContextFactory<ArmadaDbContext> factory) : IAttributeService
{
    public async Task<List<PlayerAttributeSnapshot>> GetSnapshotsByPlayerAsync(int playerId)
    {
        await using var db = await factory.CreateDbContextAsync();
        return await db.PlayerAttributeSnapshots
            .Include(s => s.Scores)
            .Where(s => s.PlayerId == playerId)
            .OrderByDescending(s => s.SnapshotDate)
            .ToListAsync();
    }

    public async Task<PlayerAttributeSnapshot?> GetLatestSnapshotAsync(int playerId)
    {
        await using var db = await factory.CreateDbContextAsync();
        return await db.PlayerAttributeSnapshots
            .Include(s => s.Scores)
            .Where(s => s.PlayerId == playerId)
            .OrderByDescending(s => s.SnapshotDate)
            .FirstOrDefaultAsync();
    }

    public async Task<PlayerAttributeSnapshot> SaveSnapshotAsync(
        int playerId, string positionCode, byte[] scores, string? notes)
    {
        await using var db = await factory.CreateDbContextAsync();
        var today = DateOnly.FromDateTime(DateTime.Today);

        var existing = await db.PlayerAttributeSnapshots
            .Include(s => s.Scores)
            .FirstOrDefaultAsync(s => s.PlayerId == playerId && s.SnapshotDate == today);

        if (existing is not null)
        {
            existing.OverallRating = AttributeWeights.ComputeOverallRating(positionCode, scores);
            existing.Notes = notes;
            db.PlayerAttributeScores.RemoveRange(existing.Scores);
            existing.Scores = scores.Select((score, index) => new PlayerAttributeScore
            {
                SnapshotId = existing.Id,
                AttributeIndex = (byte)index,
                Score = score
            }).ToList();
        }
        else
        {
            existing = new PlayerAttributeSnapshot
            {
                PlayerId = playerId,
                SnapshotDate = today,
                OverallRating = AttributeWeights.ComputeOverallRating(positionCode, scores),
                Notes = notes,
                Scores = scores.Select((score, index) => new PlayerAttributeScore
                {
                    AttributeIndex = (byte)index,
                    Score = score
                }).ToList()
            };
            db.PlayerAttributeSnapshots.Add(existing);
        }

        await db.SaveChangesAsync();
        return existing;
    }

    public async Task DeleteSnapshotAsync(int snapshotId)
    {
        await using var db = await factory.CreateDbContextAsync();
        var snapshot = await db.PlayerAttributeSnapshots
            .Include(s => s.Scores)
            .FirstOrDefaultAsync(s => s.Id == snapshotId);
        if (snapshot is null) return;
        db.PlayerAttributeSnapshots.Remove(snapshot);
        await db.SaveChangesAsync();
    }

    // Sync — no DB access, pure computation
    public decimal ComputeOverallRating(string positionCode, byte[] scores) =>
        AttributeWeights.ComputeOverallRating(positionCode, scores);

    public decimal[] ComputeAverageScores(IEnumerable<PlayerAttributeSnapshot> snapshots)
    {
        var list = snapshots.ToList();
        var result = new decimal[10];
        if (!list.Any()) return result;
        for (int i = 0; i < 10; i++)
        {
            var allScores = list.SelectMany(s => s.Scores)
                .Where(s => s.AttributeIndex == i)
                .Select(s => (decimal)s.Score).ToList();
            result[i] = allScores.Any() ? Math.Round(allScores.Average(), 1) : 0;
        }
        return result;
    }

    public decimal ComputeAverageOverall(IEnumerable<PlayerAttributeSnapshot> snapshots)
    {
        var list = snapshots.ToList();
        return list.Any() ? Math.Round(list.Average(s => s.OverallRating), 1) : 0;
    }
}
