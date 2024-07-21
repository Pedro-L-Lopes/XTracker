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
}
