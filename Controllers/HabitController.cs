using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XTracker.DTOs;
using XTracker.Services.Interfaces;

namespace XTracker.Controllers;

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
            return Ok("Hábito criado com sucesso");
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
