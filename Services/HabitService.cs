using AutoMapper;
using XTracker.DTOs;
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
        };
        await _uof.HabitRepository.Create(habitEntity);
    }

    public async Task<List<HabitDTO>> GetAllHabits()
    {
        var habits = await _uof.HabitRepository.GetAllHabits();
        var habitDTOs = habits.Select(habit =>
        {
            var habitDTO = _mapper.Map<HabitDTO>(habit);

            return habitDTO;
        }).ToList();

        return habitDTOs;
    }

    public Task<(List<HabitDTO> possibleHabits, List<int?> completedHabits)> GetHabitsForDay(string date)
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
