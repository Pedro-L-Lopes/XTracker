using HabitTracker.test.Repository;
using Microsoft.EntityFrameworkCore;
using XTracker.Context;
using XTracker.DTOs.HabitDTOs;
using XTracker.DTOs.ToDo;
using XTracker.Models.Habits;
using XTracker.Models.ToDo;
using XTracker.Repository.Interfaces;

namespace XTracker.Repository;
public class ToDoRepository : IToDoRepository
{
    private readonly AppDbContext _context;
    private readonly IUnityOfWork _uof;

    public ToDoRepository(AppDbContext context, IUnityOfWork uof)
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

    public async Task<List<ToDoTaskDTO>> GetAllTasks(Guid userId)
    {
        var tasks = await _context.Tasks
            .Where(t => t.UserId == userId)
            .Select(t => new ToDoTaskDTO
            {
                Id = t.Id,
                Title = t.Title,
                IsCompleted = t.IsCompleted,
                IsImportant = t.IsImportant,
                CreatedAt = t.CreatedAt,
            })
            .ToListAsync();

        return tasks;
    }

    public async Task CompletedTask(Guid taskId)
    {
        var existingTask = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == taskId);

        if (existingTask == null)
        {
            throw new ArgumentException("Tarefa não encontrada");
        }

        existingTask.IsCompleted = !existingTask.IsCompleted;

        await _uof.Commit();
    }
    public async Task ImportantTask(Guid taskId)
    {
        var existingTask = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == taskId);

        if (existingTask == null)
        {
            throw new ArgumentException("Tarefa não encontrada");
        }

        existingTask.IsImportant = !existingTask.IsImportant;

        await _uof.Commit();
    }
}
