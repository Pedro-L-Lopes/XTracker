using System.Text.Json.Serialization;
using XTracker.Models.Habits;

namespace XTracker.DTOs;
public class HabitDTO
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public DateTime? CreatedDate { get; set; }

    public List<DayHabit>? DayHabits { get; set; }
    public List<int>? WeekDays { get; set; }
}
