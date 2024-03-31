using XTracker.DTOs;

namespace XTracker.Services.Interfaces;
public interface IHabitService
{
    Task Create(HabitDTO habitDTO);
    Task<List<HabitDTO>> GetAllHabits();
    Task<(List<HabitDTO> possibleHabits, List<int?> completedHabits)> GetHabitsForDay(string date);
    Task<List<SummaryDTO>> GetSummary();
    Task<( int available, int completed)> GetHabitMetrics(int habitId);
    Task ToggleHabitForDay(int habitId, DateTime date);

    Task Delete(int habitId);
}
