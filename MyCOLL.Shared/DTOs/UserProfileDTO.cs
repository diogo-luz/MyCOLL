using System.ComponentModel.DataAnnotations;

namespace MyCOLL.Shared.DTOs;

public class UserProfileDTO {
    public string Id { get; set; } = string.Empty;

    [Required(ErrorMessage = "O nome é obrigatório")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "O apelido é obrigatório")]
    public string Apelido { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty; // Read-only na edição normal

    public long? NIF { get; set; }
    public string? Rua { get; set; }
    public string? Localidade { get; set; }
    public string? CodigoPostal { get; set; }
    public string? Pais { get; set; }
    public string? Telefone { get; set; }

    public byte[]? Fotografia { get; set; }

    // Tipo de utilizador para mostrar no perfil (readonly)
    public string? TipoUtilizador { get; set; }
}
