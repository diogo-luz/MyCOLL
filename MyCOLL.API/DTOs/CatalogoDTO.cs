namespace MyCOLL.API.DTOs;

public class CategoriaDTO {
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public int Ordem { get; set; }
    public byte[]? Imagem { get; set; }
}

public class TipoColecionavelDTO {
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public int Ordem { get; set; }
    public byte[]? Imagem { get; set; }
}

public class PaisDTO {
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? CodigoISO { get; set; }
    public byte[]? Bandeira { get; set; }
}

public class ModoDisponibilizacaoDTO {
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
}
