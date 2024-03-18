namespace XTracker.Repository.Interfaces;
public interface IUnityOfWork
{
    IHabitRepository HabitRepository { get; }
    Task Commit();
}
