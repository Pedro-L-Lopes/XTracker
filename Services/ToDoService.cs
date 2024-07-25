using AutoMapper;
using System.Security.Cryptography.X509Certificates;
using XTracker.DTOs.HabitDTOs;
using XTracker.DTOs.ToDo;
using XTracker.Models.ToDo;
using XTracker.Repository.Interfaces;
using XTracker.Services.Interfaces;

namespace XTracker.Services;

public class ToDoService : IToDoService
{
    private readonly IUnityOfWork _uof;
    private readonly IMapper _mapper;

    public ToDoService(IUnityOfWork uof, IMapper mapper)
    {
        _uof = uof;
        _mapper = mapper;

    }

    public async Task Create(ToDoTaskDTO toDoTaskDTO)
    {
        var taskEntity = new ToDoTask
        {
            Title = toDoTaskDTO.Title,
            CreatedAt = DateTime.Now.Date,
            UserId = toDoTaskDTO.UserId,
        };

        await _uof.ToDoRepository.Create(taskEntity);
    }

    public async Task<List<ToDoTaskDTO>> GetAllTasks(Guid userId)
    {
        var tasks = await _uof.ToDoRepository.GetAllTasks(userId);

        var taskDTOs = tasks.Select(task =>
        {
            var taskDTO = _mapper.Map<ToDoTaskDTO>(task);

            return taskDTO;
        }).ToList();

        return taskDTOs;
    }

    public async Task CompletedTask(Guid taskId)
    {
        await _uof.ToDoRepository.CompletedTask(taskId);
    }

    public async Task ImportantTask(Guid taskId)
    {
        await _uof.ToDoRepository.ImportantTask(taskId);
    }
}
