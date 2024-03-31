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

    public async Task<(List<HabitDTO> possibleHabits, List<int?> completedHabits)> GetHabitsForDay(string date)
    {
        if(!DateTime.TryParse(date, out DateTime parsedDate))
            throw new ArgumentException("Formato de data inválido");
        
        var possibleHabits = await _uof.HabitRepository.GetHabitsForDay(parsedDate);

        var completedHabits = await _uof.HabitRepository.GetCompletedHabitsForDay(parsedDate);

        return (_mapper.Map<List<Habit>, List<HabitDTO>>(possibleHabits), completedHabits);
    }

    public async Task<List<SummaryDTO>> GetSummary()
    {
        return await _uof.HabitRepository.GetSummary();
    }

    public async Task<(int available, int completed)> GetHabitMetrics(int habitId)
    {
        int available = await _uof.HabitRepository.GetAvailableDaysCount(habitId);

        int completed = await _uof.HabitRepository.GetCompletedCount(habitId);

        return (available, completed);

    }

    public async Task ToggleHabitForDay(int habitId, DateTime date)
    {
        await _uof.HabitRepository.ToggleHabitForDay(habitId, date);
    }

    public async Task Delete(int habitId)
    {
        await _uof.HabitRepository.Delete(habitId);
    }

}
