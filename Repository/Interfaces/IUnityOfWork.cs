namespace XTracker.Repository.Interfaces;
public interface IUnityOfWork
{
    IHabitRepository HabitRepository { get; }
    IToDoRepository ToDoRepository { get; }
    Task Commit();
}
