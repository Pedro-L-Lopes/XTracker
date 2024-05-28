using XTracker.DTOs.HabitDTOs;

namespace XTracker.Services.Interfaces;
public interface IHabitService
{
    Task Create(HabitDTO habitDTO);
    Task<List<HabitDTO>> GetAllHabits(string userId);
    Task<(List<HabitDTO> possibleHabits, List<Guid> completedHabits)> GetHabitsForDay(string date, string userId);
    Task<List<SummaryDTO>> GetSummary(string userId, int year);
    Task<(HabitDTO habit, int available, int completed)> GetHabitMetrics(Guid habitId, string startDate, string endDate);
    Task ToggleHabitForDay(Guid habitId, DateTime date);
    Task HabitEdit(Guid habitId, EditHabitDTO EdithabitDTO);

    Task Delete(Guid habitId);
}
