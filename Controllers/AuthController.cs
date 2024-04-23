﻿using Microsoft.AspNetCore.Authorization;
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

    [Authorize(Policy = "AdminOnly")]
    [HttpPost("createRole")]
    public async Task<IActionResult> CreateRole(string rolename)
    {
        var roleExists = await _roleManager.RoleExistsAsync(rolename);

        if (!roleExists)
        {
            var roleResult = await _roleManager.CreateAsync(new IdentityRole(rolename));

            if (roleResult.Succeeded)
            {
                return StatusCode(StatusCodes.Status200OK,
                    new ResponseDTO { Status = "Success", Message = $"Role {rolename} adicionada com sucesso" });
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    new ResponseDTO { Status = "Error", Message = $"Error ao adicionar a role {rolename}" });
            }
        }

        return StatusCode(StatusCodes.Status400BadRequest,
                    new ResponseDTO { Status = "Error", Message = $"A role já existe" });
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPost("addUserToRole")]
    public async Task<IActionResult> AddUserToRole(string email, string roleName)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user != null)
        {
            var result = await _userManager.AddToRoleAsync(user, roleName);
            if (result.Succeeded)
            {
                return StatusCode(StatusCodes.Status200OK,
                    new ResponseDTO { Status = "Success", Message = $"Usuário {email} adicionado a role {roleName}" });
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    new ResponseDTO { Status = "Error", Message = $"Error ao adicionar o usuário {email} a role {roleName}" });
            }
        }

        return BadRequest(new { error = "Unable to find user" });
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

        if (!await _roleManager.RoleExistsAsync("User"))
        {
            await _roleManager.CreateAsync(new IdentityRole("User"));
        }
        await _userManager.AddToRoleAsync(user, "User");

        // Gerar tokens
        var authClaims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.UserName!),
        new Claim(ClaimTypes.Email, user.Email!),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
    };

        var token = _tokenService.GenerateAccessToken(authClaims, _configuration);
        var refreshToken = _tokenService.GenerateRefreshToken();

        // Atualizar informações do usuário
        _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInMinutes"], out int RefreshTokenValidityInMinutes);
        user.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(RefreshTokenValidityInMinutes);
        user.RefreshToken = refreshToken;

        await _userManager.UpdateAsync(user);

        // Retornar tokens na resposta
        return Ok(new
        {
            user.Id,
            user.UserName,
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            RefreshToken = refreshToken,
            Expiration = token.ValidTo
        });
    }



    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
    {
        if (loginDTO == null || string.IsNullOrEmpty(loginDTO.Username) || string.IsNullOrEmpty(loginDTO.Password))
        {
            return StatusCode(StatusCodes.Status401Unauthorized,
                  new ResponseDTO { Status = "Error", Message = "Insira o nome de usuário e a senha." });
        }

        var user = await _userManager.FindByNameAsync(loginDTO.Username);
        if (user == null)
        {
            return StatusCode(StatusCodes.Status401Unauthorized,
                 new ResponseDTO { Status = "Error", Message = "Usário invalido." });
        }

        if (!await _userManager.CheckPasswordAsync(user, loginDTO.Password))
        {
            return StatusCode(StatusCodes.Status401Unauthorized,
                 new ResponseDTO { Status = "Error", Message = "Senha invalida" });
        }

        var userRoles = await _userManager.GetRolesAsync(user);
        var authClaims = new List<Claim>
        {
        new Claim(ClaimTypes.Name, user.UserName),
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(ClaimTypes.NameIdentifier, user.Id),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        foreach (var userRole in userRoles)
        {
            authClaims.Add(new Claim(ClaimTypes.Role, userRole));
        }

        var token = _tokenService.GenerateAccessToken(authClaims, _configuration);
        var refreshToken = _tokenService.GenerateRefreshToken();

        if (int.TryParse(_configuration["JWT:RefreshTokenValidityInMinutes"], out int refreshTokenValidityInMinutes))
        {
            user.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(refreshTokenValidityInMinutes);
        }

        user.RefreshToken = refreshToken;
        await _userManager.UpdateAsync(user);

        return Ok(new
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            RefreshToken = refreshToken,
            UserId = user.Id,
            Expiration = token.ValidTo
        });
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken(TokenDTO tokenDTO)
    {
        if (tokenDTO is null)
        {
            return BadRequest("Requisição invalida");
        }

        string? accessToken = tokenDTO.AccessToken ?? throw new ArgumentNullException(nameof(tokenDTO));

        string? refreshToken = tokenDTO.RefreshToken ?? throw new ArgumentException(nameof(tokenDTO));

        var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken!, _configuration);

        if (principal == null)
        {
            return BadRequest("Acesso invalido toke/refresh token");
        }

        string userName = principal.Identity.Name;

        var user = await _userManager.FindByNameAsync(userName!);

        if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
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

    [Authorize(Policy = "AdminOnly")]
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
