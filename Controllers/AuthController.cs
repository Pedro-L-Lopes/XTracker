using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using XTracker.DTOs.UserDTOs;
using XTracker.Models.Users;
using XTracker.Services.Interfaces;

namespace XTracker.Controllers;

[Route("/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ITokenService _tokenService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;

    public AuthController(ITokenService tokenService, UserManager<ApplicationUser> userManager,
                            RoleManager<IdentityRole> roleManager, IConfiguration configuration)
    {
        _tokenService = tokenService;
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
    {
        var userExists = await _userManager.FindByNameAsync(registerDTO.Username!);

        if (userExists != null)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                   new ResponseDTO { Status = "Error", Message = "Usuário já existente, faça login ou tente com outro email" });
        }

        ApplicationUser user = new()
        {
            Email = registerDTO.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = registerDTO.Username
        };

        var result = await _userManager.CreateAsync(user, registerDTO.Password!);

        if (!result.Succeeded)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                   new ResponseDTO { Status = "Error", Message = "O registro falhou, por favor tente novamente mais tarde" });
        }

        return Ok(new ResponseDTO { Status = "Success", Message = "Usuário criado com sucesso" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
    {
        var user = await _userManager.FindByNameAsync(loginDTO.Username!);

        if (user is not null && await _userManager.CheckPasswordAsync(user, loginDTO.Password!))
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var token = _tokenService.GenerateAccessToken(authClaims, _configuration);

            var refreshToken = _tokenService.GenerateRefreshToken();

            _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInMinutes"], out int RefreshTokenValidityInMinutes);

            user.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(RefreshTokenValidityInMinutes);

            user.RefreshToken = refreshToken;

            await _userManager.UpdateAsync(user);

            return Ok(new
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken,
                Expiration = token.ValidTo
            });
        }

        return Unauthorized();
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken(TokenDTO tokenDTO)
    {
        if(tokenDTO is null)
        {
            return BadRequest("Requisição invalida");
        }

        string? accessToken = tokenDTO.AccessToken ?? throw new ArgumentNullException(nameof(tokenDTO));

        string? refreshToken = tokenDTO.RefreshToken ?? throw new ArgumentException(nameof(tokenDTO));

        var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken!, _configuration);

        if(principal == null)
        {
            return BadRequest("Acesso invalido toke/refresh token");
        }

        string userName = principal.Identity.Name;

        var user = await _userManager.FindByNameAsync(userName!);

        if(user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
        {
            return BadRequest("Acesso invalido toke/refresh token");
        }

        var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims.ToList(), _configuration);

        var newRefreshToken = _tokenService.GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        await _userManager.UpdateAsync(user);

        return new ObjectResult(new
        {
            accessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
            refreshToken = newRefreshToken,
        });
    }

    [Authorize]
    [HttpPost("revoke/{username}")]
    public async Task<IActionResult> Revoke(string username)
    {
        var user = await _userManager.FindByNameAsync(username);

        if (user == null) return BadRequest("usuário invalido");

        user.RefreshToken = null;

        await _userManager.UpdateAsync(user);

        return NoContent();
    }
}
