﻿namespace XTracker.Models.Habits
{
    public class Habit
    {
        public Guid Id { get; set; }

        public string UserId { get; set; } = string.Empty;
        public string? Title { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<DayHabit>? DayHabits { get; set; }
        public List<HabitWeekDay> WeekDays { get; set; } = [];
    }
}
