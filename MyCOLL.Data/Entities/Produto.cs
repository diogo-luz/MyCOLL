using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;
using MyCOLL.Data.Data;

namespace MyCOLL.Data.Entities;

/// <summary>
/// Produto/Colecionável principal
/// </summary>
public class Produto {
    public int Id { get; set; }

    [Required(ErrorMessage = "O nome do produto é obrigatório.")]
    [StringLength(100)]
    public string? Nome { get; set; }

    [Required(ErrorMessage = "A descrição é obrigatória.")]
    [StringLength(500)]
    public string? Descricao { get; set; }

    /// <summary>
    /// Preço definido pelo Fornecedor
    /// </summary>
    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public decimal PrecoBase { get; set; }

    /// <summary>
    /// Percentagem aplicada pelo Admin/Funcionário
    /// </summary>
    [Column(TypeName = "decimal(5,2)")]
    public decimal Percentagem { get; set; } = 0;

    /// <summary>
    /// Preço final = PrecoBase + (PrecoBase * Percentagem / 100)
    /// </summary>
    [Column(TypeName = "decimal(10,2)")]
    public decimal PrecoFinal { get; set; }

    public int Stock { get; set; } = 1;

    /// <summary>
    /// true = para venda, false = apenas para listagem/exibição
    /// </summary>
    public bool ParaVenda { get; set; } = true;

    /// <summary>
    /// Visível aos clientes (após ativação pelo Admin/Funcionário)
    /// </summary>
    public bool Activo { get; set; } = false;

    /// <summary>
    /// Pendente, Activo, Suspenso
    /// </summary>
    [StringLength(20)]
    public string Estado { get; set; } = "Pendente";

    public int? Ano { get; set; }

    [DataType(DataType.ImageUrl)] public string? UrlImagem { get; set; }

    public byte[]? Imagem { get; set; }

    [NotMapped] [JsonIgnore] public IFormFile? ImageFile { get; set; }

    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

    // FK - Fornecedor
    [Required] public string FornecedorId { get; set; } = null!;
    [JsonIgnore] public ApplicationUser? Fornecedor { get; set; }

    // FK - TipoColecionavel
    [Required] public int TipoColecionavelId { get; set; }
    public TipoColecionavel? TipoColecionavel { get; set; }

    // FK - Pais (opcional)
    public int? PaisId { get; set; }
    public Pais? Pais { get; set; }

    // FK - Categoria (opcional)
    public int? CategoriaId { get; set; }
    public Categoria? Categoria { get; set; }

    // FK - ModoDisponibilizacao
    [Required] public int ModoDisponibilizacaoId { get; set; }
    public ModoDisponibilizacao? ModoDisponibilizacao { get; set; }

    // Navigation property
    [JsonIgnore] public ICollection<ItemEncomenda>? ItensEncomenda { get; set; }
}
