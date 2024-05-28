using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using XTracker.Models.Habits;

namespace XTracker.DTOs.HabitDTOs;
public class HabitDTO
{
    public Guid Id { get; set; }
    public string UserId { get; set; }

    [Required(ErrorMessage = "Insira o titulo do hábito")]
    public string? Title { get; set; }
    public DateTime CreatedAt { get; set; }

    [JsonIgnore]
    public List<DayHabit>? DayHabits { get; set; }

    [Required(ErrorMessage = "Informe os dias da semana que o hábito ficará disponível.")]
    public List<int> WeekDays { get; set; }
}
