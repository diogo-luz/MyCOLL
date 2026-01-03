using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MyCOLL.Data.Entities;

/// <summary>
/// Item individual de uma encomenda
/// </summary>
public class ItemEncomenda {
    public int Id { get; set; }

    [Required]
    public int Quantidade { get; set; } = 1;

    /// <summary>
    /// Preço unitário no momento da compra
    /// </summary>
    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public decimal PrecoUnitario { get; set; }

    // FK - Encomenda
    [Required]
    public int EncomendaId { get; set; }
    [JsonIgnore]
    public Encomenda? Encomenda { get; set; }

    // FK - Produto
    [Required]
    public int ProdutoId { get; set; }
    public Produto? Produto { get; set; }
}
