using AutoMapper;
using XTracker.DTOs.HabitDTOs;
using XTracker.Models.Habits;

namespace XTracker.DTOs.Mappings;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Habit, HabitDTO>().ReverseMap();
    }

}
