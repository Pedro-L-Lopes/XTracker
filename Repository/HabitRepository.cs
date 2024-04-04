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
                 CreatedAt = h.CreatedAt,
                 WeekDays = _context.HabitWeekDays
                        .Where(hwd => hwd.HabitId == h.Id)
                        .Select(hwd => hwd.WeekDay.GetValueOrDefault())
                        .ToList(),
             }).ToListAsync();

        return habits;
    }

    public async Task<List<Habit>> GetHabitsForDay(DateTime date)
    {
        return await _context.Habits
         .Where(h => h.CreatedAt.Date <= date.Date && h.WeekDays.Any(w => w.WeekDay == (int)date.DayOfWeek))
         .AsNoTracking()
         .ToListAsync();
    }

    public async Task<List<int?>> GetCompletedHabitsForDay(DateTime date)
    {
        return await _context.DayHabits
            .Where(dh => dh.Day.Date == date.Date)
            .Select(dh => dh.HabitId)
            .Distinct()
            .ToListAsync();
    }

    public async Task<List<SummaryDTO>> GetSummary()
    {
        var summary = await _context.Days
            .Select(d => new SummaryDTO
            {
                Id = d.Id,
                Date = d.Date,
                Completed = d.DayHabits.Count,
                Amount = _context.HabitWeekDays
                    .Count(hwd => d.Date.HasValue &&
                            hwd.WeekDay == (int)d.Date.Value.DayOfWeek &&
                            hwd.Habit.CreatedAt.Date <= d.Date.Value.Date)
            })
            .ToListAsync();

        return summary;
    }

    // Repository
    public async Task<(HabitDTO habit, int available, int completed)> GetHabitMetrics(int habitId)
    {
        var habit = await _context.Habits
            .Where(h => h.Id == habitId)
            .Select(h => new
            {
                Habit = new HabitDTO
                {
                    Id = h.Id,
                    Title = h.Title,
                    CreatedAt = h.CreatedAt,
                    WeekDays = _context.HabitWeekDays
                        .Where(hwd => hwd.HabitId == h.Id)
                        .Select(hwd => hwd.WeekDay.GetValueOrDefault())
                        .ToList()
                },
                AvailableDaysCount = _context.HabitWeekDays
                    .Where(hwd => hwd.HabitId == habitId)
                    .Count(),
                CompletedCount = _context.DayHabits
                    .Where(dh => dh.HabitId == habitId)
                    .Count()
            })
            .FirstOrDefaultAsync();

        return (habit?.Habit, habit?.AvailableDaysCount ?? 0, habit?.CompletedCount ?? 0);
    }


    public async Task ToggleHabitForDay(int habitId, DateTime date)
    {
        var day = await _context.Days.FirstOrDefaultAsync(d => d.Date == date);

        if (day == null)
        {
            day = new Day { Date = date };
            _context.Days.Add(day);
            await _uof.Commit();
        }

        var existingDayHabit = await _context.DayHabits
            .FirstOrDefaultAsync(dh => dh.DayId == day.Id && dh.HabitId == habitId);

        if (existingDayHabit != null)
        {
            _context.DayHabits.Remove(existingDayHabit);
        }
        else
        {
            var newDayHabit = new DayHabit { DayId = day.Id, HabitId = habitId };
            _context.DayHabits.Add(newDayHabit);
        }

        await _uof.Commit();
    }

    public async Task Delete(int habitId)
    {
        var habit = await _context.Habits.FindAsync(habitId);

        if (habit != null)
        {
            _context.Habits.Remove(habit);
            await _uof.Commit();
        }
        else
        {
            throw new ArgumentException("Hábito não encontrado");
        }
    }
}
