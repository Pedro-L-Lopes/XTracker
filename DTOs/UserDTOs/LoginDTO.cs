using System.ComponentModel.DataAnnotations;

namespace XTracker.DTOs.UserDTOs;
public class LoginDTO
{
    [Required(ErrorMessage = "Insira um nome de usuário")]
    public string? Username { get; set; }

    [Required(ErrorMessage = "Insira a senha corretamente")]
    public string? Password { get; set; }
}
