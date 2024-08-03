using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.RegularExpressions;
using XTracker.DTOs.UserDTOs;
using XTracker.Models.Users;
using XTracker.Services.Interfaces;

namespace XTracker.Controllers
{
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

        /// <summary>
        /// Create a new role.
        /// </summary>
        /// <param name="rolename"></param>
        /// <returns></returns>
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
                        new ResponseDTO { Status = "Success", Message = $"Role {rolename} added successfully" });
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest,
                        new ResponseDTO { Status = "Error", Message = $"Error adding role {rolename}" });
                }
            }

            return StatusCode(StatusCodes.Status400BadRequest,
                        new ResponseDTO { Status = "Error", Message = $"The role already exists" });
        }

        /// <summary>
        /// Add a user to a role.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
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
                        new ResponseDTO { Status = "Success", Message = $"User {email} added to role {roleName}" });
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest,
                        new ResponseDTO { Status = "Error", Message = $"Error adding user {email} to role {roleName}" });
                }
            }

            return BadRequest(new { error = "Unable to find user" });
        }

        /// <summary>
        /// Register a new user.
        /// </summary>
        /// <param name="registerDTO">Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, and one special character.</param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
        {
            // Check if the username contains only letters and numbers
            if (!Regex.IsMatch(registerDTO.Username!, @"^[a-zA-Z0-9]+$"))
            {
                return BadRequest(new ResponseDTO { Status = "Error", Message = "Não utilize espaços ou caracteres especiais no nome de usuário" });
            }

            // Check if the password meets complexity criteria
            if (!Regex.IsMatch(registerDTO.Password!, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).{8,}$"))
            {
                return BadRequest(new ResponseDTO { Status = "Error", Message = "A senha não atende aos critérios!" });
            }

            // Check if the username is already taken
            var userExists = await _userManager.FindByNameAsync(registerDTO.Username!);
            if (userExists != null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                       new ResponseDTO { Status = "Error", Message = "Esse nome de usuário já está sendo utilizado, por favor utilize outro" });
            }

            // Check if the email is already taken
            var emailExists = await _userManager.FindByEmailAsync(registerDTO.Email!);
            if (emailExists != null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                       new ResponseDTO { Status = "Error", Message = "Esse email já está sendo utilizado, por favor utilize outro" });
            }

            ApplicationUser user = new()
            {
                Email = registerDTO.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = registerDTO.Username,
                CreatedAt = DateTime.UtcNow
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

            // Generate tokens
            var authClaims = new List<Claim>
            {
               new Claim(ClaimTypes.Name, user.UserName!),
               new Claim(ClaimTypes.Email, user.Email!),
               new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var token = _tokenService.GenerateAccessToken(authClaims, _configuration);
            var refreshToken = _tokenService.GenerateRefreshToken();

            // Update user information
            _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"], out int RefreshTokenValidityInDays);
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(RefreshTokenValidityInDays);
            user.RefreshToken = refreshToken;

            await _userManager.UpdateAsync(user);

            // Return tokens in the response
            return Ok(new
            {
                userId = user.Id,
                userName = user.UserName,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken,
                Expiration = token.ValidTo,
                CreatedAt = user.CreatedAt,
            });
        }


        /// <summary>
        /// Log in a user.
        /// </summary>
        /// <param name="loginDTO"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            if (loginDTO == null || string.IsNullOrEmpty(loginDTO.Email) || string.IsNullOrEmpty(loginDTO.Password))
            {
                return StatusCode(StatusCodes.Status401Unauthorized,
                      new ResponseDTO { Status = "Error", Message = "Insira o email e a senha" });
            }

            var user = await _userManager.FindByEmailAsync(loginDTO.Email);

            if (user == null)
            {
                return StatusCode(StatusCodes.Status401Unauthorized,
                     new ResponseDTO { Status = "Error", Message = "Email invalido" });
            }

            if (!await _userManager.CheckPasswordAsync(user, loginDTO.Password))
            {
                return StatusCode(StatusCodes.Status401Unauthorized,
                     new ResponseDTO { Status = "Error", Message = "Senha invalida" });
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var token = _tokenService.GenerateAccessToken(authClaims, _configuration);
            var refreshToken = _tokenService.GenerateRefreshToken();

            if (int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays))
            {
                user.RefreshTokenExpiryTime = DateTime.Now.AddDays(refreshTokenValidityInDays);
            }

            user.RefreshToken = refreshToken;
            await _userManager.UpdateAsync(user);

            return Ok(new
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken,
                UserId = user.Id,
                UserName = user.UserName,
                Expiration = token.ValidTo,
                CreatedAt = user.CreatedAt
            });
        }

        /// <summary>
        /// Refresh the access token using a refresh token.
        /// </summary>
        /// <param name="tokenDTO"></param>
        /// <returns></returns>
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(TokenDTO tokenDTO)
        {
            if (tokenDTO is null)
            {
                return BadRequest("Invalid request");
            }

            string? accessToken = tokenDTO.AccessToken ?? throw new ArgumentNullException(nameof(tokenDTO));

            string? refreshToken = tokenDTO.RefreshToken ?? throw new ArgumentException(nameof(tokenDTO));

            var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken!, _configuration);

            if (principal == null)
            {
                return BadRequest("Invalid access token/refresh token");
            }

            string userName = principal.Identity!.Name!;

            var user = await _userManager.FindByNameAsync(userName!);

            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return BadRequest("Invalid access token/refresh token");
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

        /// <summary>
        /// Revoke the refresh token of a user.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [Authorize(Policy = "AdminOnly")]
        [HttpPost("revoke/{username}")]
        public async Task<IActionResult> Revoke(string username)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user == null) return BadRequest("Invalid user");

            user.RefreshToken = null;

            await _userManager.UpdateAsync(user);

            return NoContent();
        }
    }
}
