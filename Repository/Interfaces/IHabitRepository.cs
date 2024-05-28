using Microsoft.AspNetCore.Mvc;
using XTracker.DTOs.HabitDTOs;
using XTracker.Models.Habits;

namespace XTracker.Repository.Interfaces;
public interface IHabitRepository
{
    Task<Habit> Create(Habit habit);

    Task<List<HabitDTO>> GetAllHabits(string userId);

    Task<List<Habit>> GetHabitsForDay(DateTime date, string userId);
    Task<List<Guid>> GetCompletedHabitsForDay(DateTime date, string userId);
    public Task<(HabitDTO habit, int available, int completed)> GetHabitMetrics(Guid habitId, DateTime startDate, DateTime endDate);

    Task<List<SummaryDTO>> GetSummary(string userId, int year);

    Task ToggleHabitForDay(Guid habitId, DateTime date);
    Task EditHabit(Guid habitId, Habit habit);

    Task Delete(Guid habitId);
}
