using ArmadaEsports.Core.Models;

namespace ArmadaEsports.Core.Interfaces;

public interface IAttendanceService
{
    Task<List<TrainingAttendance>> GetByMonthAsync(int year, int month);
    Task UpsertBatchAsync(Dictionary<int, bool> checks, DateOnly date);
    Task<Dictionary<int, (int Present, int Total)>> GetMonthlyStatsAsync(int year, int month);
}
