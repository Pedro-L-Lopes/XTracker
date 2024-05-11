using System.ComponentModel.DataAnnotations;

namespace XTracker.DTOs.UserDTOs;
public class LoginDTO
{
    [Required(ErrorMessage = "Insira o email")]
    public string? Email{ get; set; }

    [Required(ErrorMessage = "Insira a senha corretamente")]
    public string? Password { get; set; }
}
