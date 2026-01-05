using System.ComponentModel.DataAnnotations;

namespace MyCOLL.Shared.DTOs;

public class ProdutoDTO {
    public int Id { get; set; }

    [Required(ErrorMessage = "O nome é obrigatório", AllowEmptyStrings = false)]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "A descrição é obrigatória", AllowEmptyStrings = false)]
    public string? Descricao { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "O preço deve ser maior que 0")]
    public decimal PrecoBase { get; set; }

    public decimal PrecoFinal { get; set; }
    public decimal Percentagem { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "O stock não pode ser negativo")]
    public int Stock { get; set; }

    public string? Estado { get; set; }

    [Range(1900, 2100, ErrorMessage = "Ano inválido")]
    public int? Ano { get; set; }

    public bool ParaVenda { get; set; }
    public bool Activo { get; set; }
    public string? UrlImagem { get; set; }
    public byte[]? Imagem { get; set; }

    // IDs de navegação (para edição)
    [Required(ErrorMessage = "A categoria é obrigatória")]
    public int? CategoriaId { get; set; }

    [Required(ErrorMessage = "O tipo colecionável é obrigatório")]
    public int? TipoColecionavelId { get; set; } // Pode ser int se FK obrigatória

    [Required(ErrorMessage = "O país é obrigatório")]
    public int? PaisId { get; set; }

    [Required(ErrorMessage = "O modo de disponibilização é obrigatório")]
    public int? ModoDisponibilizacaoId { get; set; }

    // Navegação
    public string? CategoriaNome { get; set; }
    public string? TipoColecionavelNome { get; set; }
    public string? PaisNome { get; set; }
    public string? ModoDisponibilizacaoNome { get; set; }
    public string? FornecedorNome { get; set; }
}

public class ProdutoCreateDTO {
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public decimal PrecoBase { get; set; }
    public int Stock { get; set; }
    public string? Estado { get; set; }
    public int? Ano { get; set; }
    public bool ParaVenda { get; set; }
    public string? UrlImagem { get; set; }
    public byte[]? Imagem { get; set; }

    // IDs de navegação
    public int? CategoriaId { get; set; }
    public int TipoColecionavelId { get; set; }
    public int? PaisId { get; set; }
    public int ModoDisponibilizacaoId { get; set; }
}
