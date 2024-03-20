using Microsoft.EntityFrameworkCore;
using XTracker.Context;
using XTracker.DTOs;
using XTracker.Models.Habits;
using XTracker.Repository.Interfaces;

namespace XTracker.Repository;
public class HabitRepository : IHabitRepository
{
    private readonly AppDbContext _context;
    private readonly IUnityOfWork _uof;

    public HabitRepository(AppDbContext context, IUnityOfWork uof)
    {
        _context = context;
        _uof = uof;
    }

    public async Task<Habit> Create(Habit habit)
    {
        _context.Habits.Add(habit);
        await _uof.Commit();
        return habit;
    }

    public async Task<List<HabitDTO>> GetAllHabits()
    {
        var habits = await _context.Habits
             .Select(h => new HabitDTO
             {
                 Id = h.Id,
                 Title = h.Title,
                 CreatedDate = h.CreatedAt,
                 WeekDays = _context.HabitWeekDays
                        .Where(hwd => hwd.HabitId == h.Id)
                        .Select(hwd => hwd.WeekDay.GetValueOrDefault())
                        .ToList(),
             }).ToListAsync();

        return habits;
    }

    public Task<List<int?>> GetCompletedHabitsForDay(DateTime date)
    {
        throw new NotImplementedException();
    }

    public Task<List<Habit>> GetHabitsForDay(DateTime date)
    {
        throw new NotImplementedException();
    }

    public Task ToggleHabitForDay(int habitId, DateTime date)
    {
        throw new NotImplementedException();
    }

    public Task Delete(int id)
    {
        throw new NotImplementedException();
    }
}
