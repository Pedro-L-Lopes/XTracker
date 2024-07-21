using HabitTracker.test.Repository;
using XTracker.Context;
using XTracker.Models.ToDo;
using XTracker.Repository.Interfaces;

namespace XTracker.Repository;
public class ToDoRepository : IToDoRepository
{
    private readonly AppDbContext _context;
    private readonly IUnityOfWork _uof;

    public ToDoRepository (AppDbContext context, IUnityOfWork uof)
    {
        _context = context;
        _uof = uof;
    }

    public async Task<ToDoTask> Create(ToDoTask task)
    {
        _context.Tasks.Add(task);
        await _uof.Commit();
        return task;
    }
}
