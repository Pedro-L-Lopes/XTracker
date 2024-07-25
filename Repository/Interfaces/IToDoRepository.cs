using XTracker.DTOs.ToDo;
using XTracker.Models.ToDo;

namespace XTracker.Repository.Interfaces;
public interface IToDoRepository
{
    Task<ToDoTask> Create(ToDoTask task);
    Task<List<ToDoTaskDTO>> GetAllTasks(Guid userId);
    Task CompletedTask(Guid taskId);
    Task ImportantTask(Guid taskId);
}
