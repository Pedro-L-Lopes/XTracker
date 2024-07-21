using XTracker.DTOs.ToDo;

namespace XTracker.Services.Interfaces;
public interface IToDoService
{
    Task Create(ToDoTaskDTO toDoTaskDTO);
}
