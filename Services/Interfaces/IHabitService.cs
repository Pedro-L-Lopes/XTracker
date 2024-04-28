using XTracker.DTOs;

namespace XTracker.Services.Interfaces;
public interface IHabitService
{
    Task Create(HabitDTO habitDTO);
    Task<List<HabitDTO>> GetAllHabits(string userId);
    Task<(List<HabitDTO> possibleHabits, List<int?> completedHabits)> GetHabitsForDay(string date, string userId);
    Task<List<SummaryDTO>> GetSummary(string userId);
    Task<(HabitDTO habit, int available, int completed)> GetHabitMetrics(int habitId);
    Task ToggleHabitForDay(int habitId, DateTime date);

    Task Delete(int habitId);
}
