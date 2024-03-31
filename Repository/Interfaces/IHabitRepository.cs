using XTracker.DTOs;
using XTracker.Models.Habits;

namespace XTracker.Repository.Interfaces;
public interface IHabitRepository
{
    Task<Habit> Create(Habit habit);

    Task<List<HabitDTO>> GetAllHabits();

    Task<List<Habit>> GetHabitsForDay(DateTime date);
    Task<List<int?>> GetCompletedHabitsForDay(DateTime date);

    Task<int> GetAvailableDaysCount(int habitId);
    Task<int> GetCompletedCount(int habitId);

    Task<List<SummaryDTO>> GetSummary();

    Task ToggleHabitForDay(int habitId, DateTime date);

    Task Delete(int habitId);
}
