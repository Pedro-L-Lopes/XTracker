using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using XTracker.DTOs.HabitDTOs;
using XTracker.Services.Interfaces;

namespace XTracker.Controllers
{
    [ApiConventionType(typeof(DefaultApiConventions))]
    [Route("/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
        /// <param name="userId">The ID of the user whose habits are to be retrieved.</param>
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
        [HttpGet("allhabits")]
        public async Task<IActionResult> GetAllHabits(string userId)
        {
            try
            {
                var habits = await _habitService.GetAllHabits(userId);
                return Ok(habits);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves habits available for a specific day of the week.
        /// </summary>
        /// <param name="date">A string representing the date for which habits are requested. The expected format is "YYYY-MM-DD".</param>
        /// <param name="userId">The ID of the user whose habits are to be retrieved.</param>
        /// <returns>Returns a list of habits available and/or completed for the specified day. 
        /// {
        ///"possibleHabits": [
        ///   {
        ///        "id": 1,
        ///        "title": "Beber 2L água",
        ///        "createdAt": "2022-12-31T00:00:00",
        ///       "weekDays": []
        ///   }
        /// ],
        /// "completedHabits": [1]
        ///}
        /// </returns>
        [HttpGet("day")]
        public async Task<IActionResult> GetHabitsForDay([FromQuery] string date, string userId)
        {
            try
            {
                var (possibleHabits, completedHabits) = await _habitService.GetHabitsForDay(date, userId);
                return Ok(new { possibleHabits, completedHabits });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }
        }

        /// <summary>
        /// Returns a summary with the days, totals of habits on each day and those that have been completed
        /// </summary>
        /// <param name="userId">The ID of the user whose habits are to be summarized.</param>
        /// <param name="year">The year for which the summary is requested.</param>
        /// <returns>Returns status code 200 (OK) along with the summary if the request is successful.
        /// Example:
        ///  [{
        ///   "id": 5,
        ///   "date": "2024-03-21T00:00:00",
        ///   "completed": 2,
        ///   "amount": 5
        ///  }]
        /// </returns>
        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary([FromQuery] string userId, int year)
        {
            try
            {
                var summary = await _habitService.GetSummary(userId, year);
                return Ok(summary);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }
        }

        /// <summary>
        /// Returns the number of times the habit was available and how many times it was completed (over the entire period). 
        /// </summary>
        /// <param name="id">The ID of the habit.</param>
        /// <param name="startDate">The start date of the period for which the metrics are requested.</param>
        /// <param name="endDate">The end date of the period for which the metrics are requested.</param>
        /// <returns>Returns status code 200 (OK) along with the metrics if the request is successful.
        /// Example:
        ///  {
        ///     "available": 2,
        ///     "completed": 2
        /// }
        /// </returns>
        [HttpGet("{id}/HabitMetrics")]
        public async Task<IActionResult> GetHabitMetrics(Guid id, string startDate, string endDate)
        {
            try
            {
                var (habit, available, completed) = await _habitService.GetHabitMetrics(id, startDate, endDate);

                return Ok(new { habit, available, completed });

            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates the status of the habit (completed/not completed)
        /// </summary>
        /// <param name="id">Id of the habit to be updated</param>
        /// <param name="date">Day this habit was updated</param>
        /// <returns>Returns status code 200 (OK) with a message indicating the habit was updated successfully.</returns>
        [HttpPatch("{id}/toggle")]
        public async Task<IActionResult> ToggleHabitForDay(Guid id, [FromQuery] string date)
        {
            if (!DateTime.TryParse(date, out DateTime parsedDate))
                return BadRequest("Formato de data inválido");

            try
            {
                await _habitService.ToggleHabitForDay(id, parsedDate);
                return Ok("Hábito atualizado");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates the habit title
        /// </summary>
        /// <param name="id">Id of the habit to be updated</param>
        /// <param name="editHabitDTO">Data of habit</param>
        /// <returns>Returns status code 200 (OK) with a message indicating the habit was edited successfully.</returns>
        [HttpPut("{id}/edit")]
        public async Task<IActionResult> EditHabit(Guid id, [FromBody] UpdateHabitDTO editHabitDTO)
        {
            try
            {
                await _habitService.HabitEdit(id, editHabitDTO);
                return Ok("Hábito editado com sucesso.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Ocorreu um erro ao editar o hábito.");
            }
        }

        /// <summary>
        /// Delete habit
        /// </summary>
        /// <param name="id">Id of the habit to be excluded</param>
        /// <returns>Returns status code 200 (OK) with a message indicating the habit was deleted successfully.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _habitService.Delete(id);
                return Ok("Hábito excluído com sucesso!");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
