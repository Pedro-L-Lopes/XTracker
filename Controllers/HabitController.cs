using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using XTracker.DTOs;
using XTracker.Services.Interfaces;

namespace XTracker.Controllers;

[ApiConventionType(typeof(DefaultApiConventions))]
[Route("/[controller]")]
[ApiController]
public class HabitController : ControllerBase
{
    private readonly IHabitService _habitService;

    public HabitController(IHabitService habitService)
    {
        _habitService = habitService;
    }

    /// <summary>
    /// Creates a new habit and adds it to the days of the week it will be available.
    /// </summary>
    /// <param name="habitDTO">A JSON object containing the details of the habit. The expected format is: { "title": "Exercise", "weekDays": [1, 2, 3, 4, 5] }.
    /// The "title" field is mandatory and represents the title of the habit. The "weekDays" field is a list of numbers representing the days of the week (1 for Monday, 2 for Tuesday, etc.) on which the habit will be available.</param>
    /// <returns>Returns status code 201 (Created) if the habit is successfully created.</returns>

    [HttpPost]
    public async Task<IActionResult> CreateHabit([FromBody] HabitDTO habitDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            await _habitService.Create(habitDTO);
            return StatusCode((int)HttpStatusCode.Created, "Hábito criado com sucesso");
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }


    /// <summary>
    /// Retrieves all habits from the database.
    /// </summary>
    /// <remarks>
    /// This endpoint returns a list of all habits stored in the database.
    /// </remarks>
    /// <returns>Returns a status code of 200 (OK) along with the list of habits if the request is successful.
    /// Example:
    /// 
    /// [
    ///    {
    ///        "id": 1,
    ///        "title": "Drink 2L of water",
    ///        "createdDate": "2022-12-31T00:00:00",
    ///        "weekDays": [1, 2, 3]
    ///    }
    /// ]
    /// </returns>
    [HttpGet("allHabits")]
    public async Task<IActionResult> GetAllHabits()
    {
        try
        {
            var habits = await _habitService.GetAllHabits();
            return Ok(habits);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
        }
    }
}
