﻿using Microsoft.AspNetCore.Mvc;
using XTracker.DTOs.HabitDTOs;
using XTracker.Models.Habits;

namespace XTracker.Repository.Interfaces;
public interface IHabitRepository
{
    Task<Habit> Create(Habit habit);

    Task<List<HabitDTO>> GetAllHabits(string userId);

    Task<List<Habit>> GetHabitsForDay(DateTime date, string userId);
    Task<List<int?>> GetCompletedHabitsForDay(DateTime date, string userId);
    public Task<(HabitDTO habit, int available, int completed)> GetHabitMetrics(int habitId);

    Task<List<SummaryDTO>> GetSummary(string userId, int year);

    Task ToggleHabitForDay(int habitId, DateTime date);
    Task EditHabit(int habitId, Habit habit);

    Task Delete(int habitId);
}
