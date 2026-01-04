using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using MyCOLL.Data.Data;

namespace MyCOLL.Data.Entities;

/// <summary>
/// Encomenda feita por um Cliente
/// </summary>
public class Encomenda {
    public int Id { get; set; }

    public DateTime DataEncomenda { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Pendente, Confirmada, Expedida, Entregue, Rejeitada
    /// </summary>
    [Required(ErrorMessage = "O estado é obrigatório.")]
    [StringLength(20)]
    public string Estado { get; set; } = "Pendente";

    [Column(TypeName = "decimal(10,2)")] public decimal Total { get; set; }

    [StringLength(500)] public string? Observacoes { get; set; }

    // FK - Cliente
    [Required(ErrorMessage = "O cliente é obrigatório.")]
    public string ClienteId { get; set; } = null!;

    [JsonIgnore] public ApplicationUser? Cliente { get; set; }

    // Navigation property
    public ICollection<ItemEncomenda>? Itens { get; set; }
}
