using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XTracker.Services.Interfaces;

namespace XTracker.Controllers;

[ApiConventionType(typeof(DefaultApiConventions))]
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
    public async Task <IActionResult> GetUserDetails(string userId)
    {
        try
        {
            var userDetails = await _userService.UserDetails(userId);
            return Ok(userDetails);
        }catch (Exception ex)
        {
            return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
        }
    }

}
