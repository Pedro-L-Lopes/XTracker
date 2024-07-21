using AutoMapper;
using System.Security.Cryptography.X509Certificates;
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
}
