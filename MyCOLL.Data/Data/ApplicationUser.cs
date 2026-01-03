using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using MyCOLL.Data.Entities;

namespace MyCOLL.Data.Data;

/// <summary>
/// Utilizador da aplicação com campos adicionais para Cliente/Fornecedor
/// </summary>
public class ApplicationUser : IdentityUser {
    [StringLength(100)] public string? Nome { get; set; }

    [StringLength(100)] public string? Apelido { get; set; }

    public long? NIF { get; set; }

    [StringLength(200)] public string? Rua { get; set; }

    [StringLength(10)] public string? CodigoPostal { get; set; }

    [StringLength(100)] public string? Localidade { get; set; }

    [StringLength(100)] public string? Pais { get; set; }

    /// <summary>
    /// Cliente, Fornecedor (usado apenas no frontend; os roles são geridos pelo Identity)
    /// </summary>
    [StringLength(20)]
    public string? TipoUtilizador { get; set; }

    /// <summary>
    /// Conta activa (Pendente = false, Activo = true)
    /// </summary>
    public bool Activo { get; set; } = false;

    public byte[]? Fotografia { get; set; }

    // Navigation properties
    [JsonIgnore] public ICollection<Produto>? ProdutosFornecidos { get; set; }

    [JsonIgnore] public ICollection<Encomenda>? Encomendas { get; set; }
}
