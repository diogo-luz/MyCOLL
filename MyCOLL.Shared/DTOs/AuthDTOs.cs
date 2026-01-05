using System.ComponentModel.DataAnnotations;

namespace MyCOLL.Shared.DTOs;

public class LoginDTO {
    [Required(ErrorMessage = "O email é obrigatório")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "A password é obrigatória")]
    public string Password { get; set; } = string.Empty;
}

public class RegisterDTO {
    [Required(ErrorMessage = "O nome é obrigatório")]
    public string Nome { get; set; } = string.Empty;

    public string? Apelido { get; set; }

    [Required(ErrorMessage = "O email é obrigatório")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "A password é obrigatória")]
    [MinLength(6, ErrorMessage = "A password deve ter pelo menos 6 caracteres")]
    public string Password { get; set; } = string.Empty;

    [Compare("Password", ErrorMessage = "As passwords não coincidem")]
    public string ConfirmPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "O tipo de utilizador é obrigatório")]
    public string TipoUtilizador { get; set; } = "Cliente";
}

public class AuthResponseDTO {
    public string AccessToken { get; set; } = string.Empty;
    public string TokenType { get; set; } = "Bearer";
    public int ExpiresIn { get; set; }
    public string Email { get; set; } = string.Empty;
}
