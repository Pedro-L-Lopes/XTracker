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

    public Task<List<HabitDTO>> GetAllHabits()
    {
        throw new NotImplementedException();
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
