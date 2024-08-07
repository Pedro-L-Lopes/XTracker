﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using XTracker.DTOs.HabitDTOs;
using XTracker.Models.Habits;
using XTracker.Repository.Interfaces;
using XTracker.Services.Interfaces;

namespace XTracker.Services;
public class HabitService : IHabitService
{
    private readonly IUnityOfWork _uof;
    private readonly IMapper _mapper;

    public HabitService(IUnityOfWork uof, IMapper mapper)
    {
        _uof = uof;
        _mapper = mapper;
    }

    public async Task Create(HabitDTO habitDTO)
    {
        var habitEntity = new Habit
        {
            Title = habitDTO.Title,
            CreatedAt = DateTime.Now.Date,
            WeekDays = habitDTO.WeekDays.Select(day => new HabitWeekDay { WeekDay = day }).ToList(),
            UserId = habitDTO.UserId,
        };
        await _uof.HabitRepository.Create(habitEntity);
    }

    public async Task<List<HabitDTO>> GetAllHabits(string userId)
    {
        var habits = await _uof.HabitRepository.GetAllHabits(userId);
        var habitDTOs = habits.Select(habit =>
        {
            var habitDTO = _mapper.Map<HabitDTO>(habit);

            return habitDTO;
        }).ToList();

        return habitDTOs;
    }

    public async Task<(List<HabitDTO> possibleHabits, List<Guid> completedHabits)> GetHabitsForDay(string date, string userId)
    {
        if (!DateTime.TryParse(date, out DateTime parsedDate))
            throw new ArgumentException("Formato de data inválido");

        var possibleHabits = await _uof.HabitRepository.GetHabitsForDay(parsedDate, userId);

        var completedHabits = await _uof.HabitRepository.GetCompletedHabitsForDay(parsedDate, userId);

        return (_mapper.Map<List<Habit>, List<HabitDTO>>(possibleHabits), completedHabits);
    }

    public async Task<List<SummaryDTO>> GetSummary(string userId, int year)
    {
        return await _uof.HabitRepository.GetSummary(userId, year);
    }

    public async Task<(HabitDTO habit, int available, int completed)> GetHabitMetrics(Guid habitId, string startDate, string endDate)
    {
        if (!DateTime.TryParse(startDate, out DateTime startDateTime))
        {
            throw new ArgumentException("Invalid start date format", nameof(startDate));
        }

        if (!DateTime.TryParse(endDate, out DateTime endDateTime))
        {
            throw new ArgumentException("Invalid end date format", nameof(endDate));
        }

        var (habit, available, completed) = await _uof.HabitRepository.GetHabitMetrics(habitId, startDateTime, endDateTime);

        return (habit, available, completed);
    }


    public async Task ToggleHabitForDay(Guid habitId, DateTime date)
    {
        await _uof.HabitRepository.ToggleHabitForDay(habitId, date);
    }

    public async Task HabitEdit(Guid habitId, UpdateHabitDTO EdithabitDTO)
    {
        var habitEntity = new Habit
        {
            Title = EdithabitDTO.Title,
        };

        await _uof.HabitRepository.EditHabit(habitId, habitEntity);
    }

    public async Task Delete(Guid habitId)
    {
        await _uof.HabitRepository.Delete(habitId);
    }
}
