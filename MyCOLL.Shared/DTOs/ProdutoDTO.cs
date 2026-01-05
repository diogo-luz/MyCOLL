namespace MyCOLL.Shared.DTOs;

public class ProdutoDTO {
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public decimal PrecoBase { get; set; }
    public decimal PrecoFinal { get; set; }
    public decimal Percentagem { get; set; }
    public int Stock { get; set; }
    public string? Estado { get; set; }
    public int? Ano { get; set; }
    public bool ParaVenda { get; set; }
    public bool Activo { get; set; }
    public string? UrlImagem { get; set; }
    public byte[]? Imagem { get; set; }

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
