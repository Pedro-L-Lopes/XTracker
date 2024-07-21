using XTracker.Models.ToDo;

namespace XTracker.Repository.Interfaces;
public interface IToDoRepository
{
    Task<ToDoTask> Create(ToDoTask task);
}
