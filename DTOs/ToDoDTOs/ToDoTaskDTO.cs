using System.ComponentModel.DataAnnotations;

namespace XTracker.DTOs.ToDo;
public class ToDoTaskDTO
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Insira o titulo da tarefa")]
    public string Title { get; set; } = string.Empty;

    public bool IsCompleted { get; set; } = false;

    public bool IsImportant { get; set; } = false;

    public DateTime CreatedAt { get; set; }

    public Guid UserId { get; set; }
}
