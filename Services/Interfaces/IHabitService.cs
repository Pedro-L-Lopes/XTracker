using XTracker.DTOs.HabitDTOs;

namespace XTracker.Services.Interfaces;
public interface IHabitService
{
    Task Create(HabitDTO habitDTO);
    Task<List<HabitDTO>> GetAllHabits(string userId);
    Task<(List<HabitDTO> possibleHabits, List<int?> completedHabits)> GetHabitsForDay(string date, string userId);
    Task<List<SummaryDTO>> GetSummary(string userId, int year);
    Task<(HabitDTO habit, int available, int completed)> GetHabitMetrics(int habitId);
    Task ToggleHabitForDay(int habitId, DateTime date);
    Task HabitEdit(int habitId, EditHabitDTO EdithabitDTO);

    Task Delete(int habitId);
}
