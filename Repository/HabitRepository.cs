using Microsoft.EntityFrameworkCore;
using XTracker.Context;
using XTracker.DTOs.HabitDTOs;
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

    public async Task<List<HabitDTO>> GetAllHabits(string userId)
    {
        var habits = await _context.Habits
             .Where(h => h.UserId == userId)
             .Select(h => new HabitDTO
             {
                 Id = h.Id,
                 Title = h.Title,
                 CreatedAt = h.CreatedAt,
                 UserId = h.UserId,
                 WeekDays = _context.HabitWeekDays
                        .Where(hwd => hwd.HabitId == h.Id)
                        .Select(hwd => hwd.WeekDay)
                        .ToList(),
             }).ToListAsync();

        return habits;
    }

    public async Task<List<Habit>> GetHabitsForDay(DateTime date, string userId)
    {
        return await _context.Habits
         .Where(h => h.CreatedAt.Date <= date.Date && h.WeekDays.Any(w => w.WeekDay == (int)date.DayOfWeek) && h.UserId == userId)
         .AsNoTracking()
         .ToListAsync();
    }

    public async Task<List<Guid>> GetCompletedHabitsForDay(DateTime date, string userId)
    {
        return await _context.DayHabits
            .Where(dh => dh.Day.Date == date.Date)
            .Join(_context.Habits,
                dh => dh.HabitId,
                h => h.Id,
                (dh, h) => new { dh, h })
            .Where(dh => dh.h.UserId == userId)
            .Select(dh => dh.dh.HabitId)
            .Distinct()
            .ToListAsync();
    }

    public async Task<List<SummaryDTO>> GetSummary(string userId, int year)
    {
        var days = await _context.Days
            .Include(d => d.DayHabits)
            .ThenInclude(dh => dh.Habit)
            .Where(d => d.DayHabits.Any(dh => dh.Habit.UserId == userId) && d.Date.HasValue && d.Date.Value.Year == year)
            .ToListAsync();

        var summary = days.Select(d => new SummaryDTO
        {
            Id = d.Id,
            Date = d.Date,
            Completed = d.DayHabits.Count(dh => dh.Habit.UserId == userId),
            Amount = _context.HabitWeekDays
                .Count(hwd => d.Date.HasValue &&
                              hwd.WeekDay == (int)d.Date.Value.DayOfWeek &&
                              hwd.Habit.UserId == userId &&
                              hwd.Habit.CreatedAt.Date <= d.Date.Value.Date)
        }).ToList();

        return summary;
    }


    public async Task<(HabitDTO habit, int available, int completed)> GetHabitMetrics(Guid habitId, DateTime startDate, DateTime endDate)
    {
        var habitInfo = await _context.Habits
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
                        .Select(hwd => hwd.WeekDay)
                        .ToList()
                },
                CompletedCount = _context.DayHabits
                    .Where(dh => dh.HabitId == habitId && dh.Day.Date >= startDate && dh.Day.Date <= endDate)
                    .Count()
            })
            .FirstOrDefaultAsync();

        if (habitInfo == null)
        {
            return (null, 0, 0);
        }

        DateTime habitStartDate = startDate > habitInfo.Habit.CreatedAt ? startDate : habitInfo.Habit.CreatedAt;
        DateTime habitEndDate = endDate < DateTime.Now.Date ? endDate : DateTime.Now.Date;

        int totalDays = (int)(habitEndDate - habitStartDate).TotalDays + 1;

        int availableDays = 0;

        for (int i = 0; i < totalDays; i++)
        {
            DateTime dayToCheck = habitStartDate.AddDays(i);
            if (habitInfo.Habit.WeekDays.Contains((int)dayToCheck.DayOfWeek))
            {
                availableDays++;
            }
        }

        return (habitInfo.Habit, availableDays, habitInfo.CompletedCount);
    }


    public async Task ToggleHabitForDay(Guid habitId, DateTime date)
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
    public async Task EditHabit(Guid habitId, Habit habit)
    {
        var existingHabit = await _context.Habits.FindAsync(habitId);

        if (existingHabit == null)
        {
            throw new ArgumentException("Hábito não encontrado");
        }

        existingHabit.Title = habit.Title;

        await _uof.Commit();
    }


    public async Task Delete(Guid habitId)
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
