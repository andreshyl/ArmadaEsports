using ArmadaEsports.Core.Interfaces;
using ArmadaEsports.Core.Models;
using ArmadaEsports.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace ArmadaEsports.Data.Services;

public class AttendanceService(IDbContextFactory<ArmadaDbContext> factory) : IAttendanceService
{
    private static bool IsTrainingDay(DateOnly d) =>
        d.DayOfWeek is DayOfWeek.Monday or DayOfWeek.Tuesday
                    or DayOfWeek.Wednesday or DayOfWeek.Thursday;

    public async Task<List<TrainingAttendance>> GetByMonthAsync(int year, int month)
    {
        await using var db = await factory.CreateDbContextAsync();
        return await db.TrainingAttendances
            .Include(a => a.Player)
            .Where(a => a.SessionDate.Year == year && a.SessionDate.Month == month)
            .ToListAsync();
    }

    public async Task UpsertBatchAsync(Dictionary<int, bool> checks, DateOnly date)
    {
        await using var db = await factory.CreateDbContextAsync();
        var playerIds = checks.Keys.ToList();

        var existing = await db.TrainingAttendances
            .Where(a => a.SessionDate == date && playerIds.Contains(a.PlayerId))
            .ToListAsync();

        foreach (var (playerId, present) in checks)
        {
            var record = existing.FirstOrDefault(a => a.PlayerId == playerId);
            if (record is null)
                db.TrainingAttendances.Add(new TrainingAttendance
                {
                    PlayerId = playerId,
                    SessionDate = date,
                    Present = present
                });
            else
                record.Present = present;
        }

        await db.SaveChangesAsync();
    }

    public async Task<Dictionary<int, (int Present, int Total)>> GetMonthlyStatsAsync(int year, int month)
    {
        await using var db = await factory.CreateDbContextAsync();
        var firstDay = new DateOnly(year, month, 1);
        var lastDay = firstDay.AddMonths(1).AddDays(-1);

        var today = DateOnly.FromDateTime(DateTime.Today);
        var cap = lastDay < today ? lastDay : today;

        int totalSessions = 0;
        for (var d = firstDay; d <= cap; d = d.AddDays(1))
            if (IsTrainingDay(d)) totalSessions++;

        var records = await db.TrainingAttendances
            .Where(a => a.SessionDate.Year == year && a.SessionDate.Month == month && a.Present)
            .GroupBy(a => a.PlayerId)
            .Select(g => new { PlayerId = g.Key, Count = g.Count() })
            .ToListAsync();

        return records.ToDictionary(
            r => r.PlayerId,
            r => (r.Count, totalSessions));
    }
}
