using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;

namespace MyCOLL.Data.Entities;

/// <summary>
/// País de origem do colecionável
/// </summary>
public class Pais {
    public int Id { get; set; }

    [Required(ErrorMessage = "O nome do país é obrigatório.")]
    [StringLength(100)]
    public string? Nome { get; set; }

    [StringLength(3)] public string? CodigoISO { get; set; }

    [Range(0, 999, ErrorMessage = "A ordem deve ser um valor entre 0 e 999.")]
    public int Ordem { get; set; } = 0;

    public byte[]? Bandeira { get; set; }

    [NotMapped] [JsonIgnore] public IFormFile? BandeiraFile { get; set; }

    // Navigation property
    [JsonIgnore] public ICollection<Produto>? Produtos { get; set; }
}
