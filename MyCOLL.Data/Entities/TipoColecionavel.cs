using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;

namespace MyCOLL.Data.Entities;

/// <summary>
/// Tipo de colecionável: Moedas, Selos, Carteiras de Fósforos, etc.
/// </summary>
public class TipoColecionavel {
    public int Id { get; set; }

    [Required(ErrorMessage = "O nome do tipo é obrigatório.")]
    [StringLength(100)]
    public string? Nome { get; set; }

    [Range(0, 999, ErrorMessage = "A ordem deve ser um valor entre 0 e 999.")]
    public int Ordem { get; set; } = 0;

    [DataType(DataType.ImageUrl)] public string? UrlImagem { get; set; }

    public byte[]? Imagem { get; set; }

    [NotMapped] [JsonIgnore] public IFormFile? ImageFile { get; set; }

    // Navigation property
    [JsonIgnore] public ICollection<Produto>? Produtos { get; set; }
}
