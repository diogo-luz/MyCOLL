using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MyCOLL.Data.Entities;

/// <summary>
/// Modo de disponibilização do produto (Venda, Listagem, etc.)
/// </summary>
public class ModoDisponibilizacao {
    public int Id { get; set; }

    [Required(ErrorMessage = "O nome do modo é obrigatório.")]
    [StringLength(100)]
    public string? Nome { get; set; }

    [StringLength(200)]
    public string? Descricao { get; set; }

    // Navigation property
    [JsonIgnore]
    public ICollection<Produto>? Produtos { get; set; }
}
