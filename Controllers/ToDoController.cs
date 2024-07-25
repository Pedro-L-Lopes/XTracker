using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using XTracker.DTOs.HabitDTOs;
using XTracker.DTOs.ToDo;
using XTracker.Services;
using XTracker.Services.Interfaces;

namespace XTracker.Controllers;

[ApiConventionType(typeof(DefaultApiConventions))]
[Route("/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ToDoController : ControllerBase
{
    private readonly IToDoService _toDoService;
    public ToDoController(IToDoService toDoService)
    {
        _toDoService = toDoService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateTask([FromBody] ToDoTaskDTO toDoTaskDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            await _toDoService.Create(toDoTaskDTO);
            return StatusCode((int)HttpStatusCode.Created, "Tarefa criada com sucesso");
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpGet("allTasks")]
    public async Task<IActionResult> GetAllTasks([FromQuery] Guid userId)
    {
        try
        {
            var tasks = await _toDoService.GetAllTasks(userId);
            return Ok(tasks);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
        }
    }
    
    [HttpPatch("isCompleted")]
    public async Task<IActionResult> CompletedTask([FromQuery] Guid taskId)
    {
        try
        {
            await _toDoService.CompletedTask(taskId);
            return Ok("Tarefa atualizada");
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "Ocorreu um erro ao atualizar a tarefa");
        }
    }
    
    [HttpPatch("isImportant")]
    public async Task<IActionResult> ImpotantTask([FromQuery] Guid taskId)
    {
        try
        {
            await _toDoService.ImportantTask(taskId);
            return Ok("Tarefa atualizada");
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "Ocorreu um erro ao atualizar a tarefa");
        }
    }
}
