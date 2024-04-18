using System.ComponentModel.DataAnnotations;

namespace XTracker.DTOs.UserDTOs;
public class RegisterDTO
{
    [EmailAddress]
    [Required(ErrorMessage = "Insira o email corretamente")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Insira um nome de usuário")]
    public string? Username { get; set; }

    [Required(ErrorMessage = "Insira a senha corretamente")]
    public string? Password { get; set; }
}
