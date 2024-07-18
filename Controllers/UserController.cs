using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Threading.Tasks;
using XTracker.DTOs.UserDTOs;
using XTracker.Services.Interfaces;

namespace XTracker.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("details")]
        public async Task<IActionResult> GetUserDetails(string userId)
        {
            try
            {
                var userDetails = await _userService.UserDetails(userId);
                return Ok(userDetails);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { status = "Error", message = $"Erro interno do servidor: {ex.Message}" });
            }
        }

        [HttpPatch("update")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDTO updateUserDTO)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { status = "Error", message = string.Join("; ", errors) });
            }

            var result = await _userService.UpdateUser(updateUserDTO);

            if (!result)
            {
                return BadRequest(new { status = "Error", message = "Falha ao atualizar usuário. Verifique a senha atual." });
            }

            return Ok(new { status = "Success", message = "Usuário atualizado com sucesso." });
        }
    }
}
