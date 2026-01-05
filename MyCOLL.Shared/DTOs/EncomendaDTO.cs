namespace MyCOLL.Shared.DTOs;

public class EncomendaDTO {
    public int Id { get; set; }
    public DateTime DataEncomenda { get; set; }
    public string? Estado { get; set; }
    public decimal Total { get; set; }
    public string? Observacoes { get; set; }

    // Cliente
    public string? ClienteNome { get; set; }
    public string? ClienteEmail { get; set; }

    // Itens
    public List<ItemEncomendaDTO> Itens { get; set; } = new();
}

public class ItemEncomendaDTO {
    public int ProdutoId { get; set; }
    public string? ProdutoNome { get; set; }
    public int Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }
    public decimal Subtotal => Quantidade * PrecoUnitario;
}

public class CreateEncomendaDTO {
    public string? Observacoes { get; set; }
    public List<CreateItemEncomendaDTO> Itens { get; set; } = new();
}

public class CreateItemEncomendaDTO {
    public int ProdutoId { get; set; }
    public int Quantidade { get; set; }
}
