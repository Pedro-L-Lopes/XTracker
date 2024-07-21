using XTracker.Context;
using XTracker.Repository.Interfaces;
using XTracker.Repository;

namespace HabitTracker.test.Repository;
public class UnityOfWork : IUnityOfWork
{
    private IHabitRepository? _habitRepository;
    private IToDoRepository? _todoRepository;

    public AppDbContext _context;

    public UnityOfWork(AppDbContext context)
    {
        _context = context;
    }

    public IHabitRepository HabitRepository
    {
        get
        {
            return _habitRepository ??= new HabitRepository(_context, this);
        }
    }

    public IToDoRepository ToDoRepository
    {
        get
        {
            return _todoRepository ??= new ToDoRepository(_context, this);
        }
    }

    public async Task Commit()
    {
        await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
