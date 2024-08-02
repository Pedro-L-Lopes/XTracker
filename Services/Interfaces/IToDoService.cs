using XTracker.DTOs.ToDo;

namespace XTracker.Services.Interfaces;
public interface IToDoService
{
    Task Create(ToDoTaskDTO toDoTaskDTO);
    Task<List<ToDoTaskDTO>> GetAllTasks(Guid userId);
    Task CompletedTask(Guid taskId);
    Task ImportantTask(Guid taskId);
    Task ChangeTaskDate(Guid taskId, DateTime date);
    Task DeleteTask(Guid taskId);
}
