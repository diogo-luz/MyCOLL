namespace MyCOLL.Shared.DTOs;

public class SupplierDashboardDTO {
    public decimal TotalGanhos { get; set; }
    public int TotalItensVendidos { get; set; }
    public int TotalProdutosAtivos { get; set; }
    public List<VendaRecenteDTO> UltimasVendas { get; set; } = new();
}

public class VendaRecenteDTO {
    public int EncomendaId { get; set; }
    public DateTime Data { get; set; }
    public string ProdutoNome { get; set; } = string.Empty;
    public int Quantidade { get; set; }
    public decimal Total { get; set; }
    public string Estado { get; set; } = string.Empty;
}
