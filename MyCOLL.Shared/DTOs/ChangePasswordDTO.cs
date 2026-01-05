using System.ComponentModel.DataAnnotations;

namespace MyCOLL.Shared.DTOs;

public class ChangePasswordDTO {
    [Required(ErrorMessage = "A password atual é obrigatória")]
    public string CurrentPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "A nova password é obrigatória")]
    [MinLength(6, ErrorMessage = "A password deve ter pelo menos 6 caracteres")]
    public string NewPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "A confirmação da password é obrigatória")]
    [Compare("NewPassword", ErrorMessage = "As passwords não coincidem")]
    public string ConfirmNewPassword { get; set; } = string.Empty;
}
