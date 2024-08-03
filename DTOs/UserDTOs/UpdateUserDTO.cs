using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace XTracker.DTOs.UserDTOs
{
    public class UpdateUserDTO
    {
        public string UserId { get; set; } = string.Empty;

        [RegularExpression("^[a-zA-Z0-9]+$", ErrorMessage = "Username só pode conter letras e números.")]
        public string? UserName { get; set; }

        [EmailAddress(ErrorMessage = "Email inválido.")]
        public string? Email { get; set; }

        [DataType(DataType.Password)]
        public string? CurrentPassword { get; set; }

        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "A nova senha deve conter no mínimo 8 caracteres.")]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\W).*$", ErrorMessage = "A nova senha deve conter pelo menos uma letra minúscula, uma letra maiúscula e um caractere especial.")]
        public string? NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "As senhas não coincidem.")]
        public string? ConfirmNewPassword { get; set; }
    }
}
