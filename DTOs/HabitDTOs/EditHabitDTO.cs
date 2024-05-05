using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using XTracker.Models.Habits;

namespace XTracker.DTOs.HabitDTOs;
public class EditHabitDTO
{
    [Required(ErrorMessage = "Insira o titulo do hábito")]
    public string? Title { get; set; }
}
