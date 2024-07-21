using AutoMapper;
using XTracker.DTOs.HabitDTOs;
using XTracker.DTOs.ToDo;
using XTracker.Models.Habits;
using XTracker.Models.ToDo;

namespace XTracker.DTOs.Mappings;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Habit, HabitDTO>().ReverseMap();
        CreateMap<ToDoTask, ToDoTaskDTO>().ReverseMap();
    }
}
