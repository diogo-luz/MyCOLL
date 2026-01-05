using System.Net.Http.Json;
using MyCOLL.Shared.DTOs;

namespace MyCOLL.Shared.Services;

public interface ICarrinhoService {
    Task AddItemAsync(ProdutoDTO produto, int quantidade = 1);
    Task RemoveItemAsync(int produtoId);
    Task UpdateQuantidadeAsync(int produtoId, int quantidade);
    Task<List<CarrinhoItem>> GetItemsAsync();
    Task<int> GetItemCountAsync();
    Task<decimal> GetTotalAsync();
    Task ClearAsync();
    Task<bool> CheckoutAsync(string? observacoes = null);
}

public class CarrinhoService : ICarrinhoService {
    private readonly ILocalStorageService _localStorage;
    private readonly HttpClient _http;
    private const string CarrinhoKey = "carrinho";

    public CarrinhoService(ILocalStorageService localStorage, HttpClient http) {
        _localStorage = localStorage;
        _http = http;
    }

    public async Task AddItemAsync(ProdutoDTO produto, int quantidade = 1) {
        var items = await GetItemsAsync();
        var existing = items.FirstOrDefault(i => i.ProdutoId == produto.Id);

        if (existing != null) {
            existing.Quantidade += quantidade;
        } else {
            items.Add(new CarrinhoItem {
                ProdutoId = produto.Id,
                Nome = produto.Nome,
                Preco = produto.PrecoFinal,
                Quantidade = quantidade,
                Imagem = produto.Imagem
            });
        }

        await _localStorage.SetItemAsync(CarrinhoKey, items);
    }

    public async Task RemoveItemAsync(int produtoId) {
        var items = await GetItemsAsync();
        items.RemoveAll(i => i.ProdutoId == produtoId);
        await _localStorage.SetItemAsync(CarrinhoKey, items);
    }

    public async Task UpdateQuantidadeAsync(int produtoId, int quantidade) {
        var items = await GetItemsAsync();
        var item = items.FirstOrDefault(i => i.ProdutoId == produtoId);
        
        if (item != null) {
            if (quantidade <= 0) {
                items.Remove(item);
            } else {
                item.Quantidade = quantidade;
            }
            await _localStorage.SetItemAsync(CarrinhoKey, items);
        }
    }

    public async Task<List<CarrinhoItem>> GetItemsAsync() {
        var items = await _localStorage.GetItemAsync<List<CarrinhoItem>>(CarrinhoKey);
        return items ?? new List<CarrinhoItem>();
    }

    public async Task<int> GetItemCountAsync() {
        var items = await GetItemsAsync();
        return items.Sum(i => i.Quantidade);
    }

    public async Task<decimal> GetTotalAsync() {
        var items = await GetItemsAsync();
        return items.Sum(i => i.Subtotal);
    }

    public async Task ClearAsync() {
        await _localStorage.RemoveItemAsync(CarrinhoKey);
    }

    public async Task<bool> CheckoutAsync(string? observacoes = null) {
        try {
            var items = await GetItemsAsync();
            if (!items.Any()) return false;

            var encomenda = new CreateEncomendaDTO {
                Observacoes = observacoes,
                Itens = items.Select(i => new CreateItemEncomendaDTO {
                    ProdutoId = i.ProdutoId,
                    Quantidade = i.Quantidade
                }).ToList()
            };

            var response = await _http.PostAsJsonAsync("api/encomendas", encomenda);
            
            if (response.IsSuccessStatusCode) {
                await ClearAsync();
                return true;
            }
            return false;
        } catch {
            return false;
        }
    }
}

public class CarrinhoItem {
    public int ProdutoId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public decimal Preco { get; set; }
    public int Quantidade { get; set; }
    public byte[]? Imagem { get; set; }
    public decimal Subtotal => Preco * Quantidade;
}
