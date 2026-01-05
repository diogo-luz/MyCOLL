using System.Net.Http.Json;
using MyCOLL.Shared.DTOs;

namespace MyCOLL.Shared.Services;

public interface IProdutoService {
    Task<List<ProdutoDTO>> GetAllAsync();
    Task<ProdutoDTO?> GetByIdAsync(int id);
    Task<List<ProdutoDTO>> GetByCategoriaAsync(int categoriaId);
    Task<List<CategoriaDTO>> GetCategoriasAsync();
    Task<List<TipoColecionavelDTO>> GetTiposAsync();
    Task<List<ProdutoDTO>> GetMyProductsAsync();
    Task<List<PaisDTO>> GetPaisesAsync();
    Task<List<ModoDisponibilizacaoDTO>> GetModosAsync();
    Task<ProdutoDTO?> CreateProductAsync(ProdutoDTO produto);
    Task<ProdutoDTO?> UpdateProductAsync(ProdutoDTO produto);
    Task<bool> DeleteProductAsync(int id);
}

public class ProdutoService : IProdutoService {
    private readonly HttpClient _http;
    private readonly ILocalStorageService _localStorage;

    public ProdutoService(HttpClient http, ILocalStorageService localStorage) {
        _http = http;
        _localStorage = localStorage;
    }

    private async Task AddAuthToken() {
        try {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            if (!string.IsNullOrEmpty(token)) {
                _http.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
        } catch {
            // Ignorar erros de JS Interop (prerendering)
        }
    }

    public async Task<List<ProdutoDTO>> GetAllAsync() {
        try {
            await AddAuthToken();
            var produtos = await _http.GetFromJsonAsync<List<ProdutoDTO>>("api/produtos");
            return produtos ?? new List<ProdutoDTO>();
        } catch {
            return new List<ProdutoDTO>();
        }
    }

    public async Task<ProdutoDTO?> GetByIdAsync(int id) {
        try {
            await AddAuthToken();
            return await _http.GetFromJsonAsync<ProdutoDTO>($"api/produtos/{id}");
        } catch {
            return null;
        }
    }

    public async Task<List<ProdutoDTO>> GetByCategoriaAsync(int categoriaId) {
        try {
            await AddAuthToken();
            var produtos = await _http.GetFromJsonAsync<List<ProdutoDTO>>($"api/produtos/categoria/{categoriaId}");
            return produtos ?? new List<ProdutoDTO>();
        } catch {
            return new List<ProdutoDTO>();
        }
    }

    public async Task<List<CategoriaDTO>> GetCategoriasAsync() {
        try {
            await AddAuthToken();
            var categorias = await _http.GetFromJsonAsync<List<CategoriaDTO>>("api/categorias");
            return categorias ?? new List<CategoriaDTO>();
        } catch {
            return new List<CategoriaDTO>();
        }
    }

    public async Task<List<TipoColecionavelDTO>> GetTiposAsync() {
        try {
            await AddAuthToken();
            var tipos = await _http.GetFromJsonAsync<List<TipoColecionavelDTO>>("api/tipos");
            return tipos ?? new List<TipoColecionavelDTO>();
        } catch {
            return new List<TipoColecionavelDTO>();
        }
    }

    public async Task<List<ProdutoDTO>> GetMyProductsAsync() {
        try {
            await AddAuthToken();
            var produtos = await _http.GetFromJsonAsync<List<ProdutoDTO>>("api/produtos/meus");
            return produtos ?? new List<ProdutoDTO>();
        } catch {
            return new List<ProdutoDTO>();
        }
    }

    public async Task<List<PaisDTO>> GetPaisesAsync() {
        try {
            await AddAuthToken();
            var paises = await _http.GetFromJsonAsync<List<PaisDTO>>("api/paises");
            return paises ?? new List<PaisDTO>();
        } catch {
            return new List<PaisDTO>();
        }
    }

    public async Task<List<ModoDisponibilizacaoDTO>> GetModosAsync() {
        try {
            await AddAuthToken();
            var modos = await _http.GetFromJsonAsync<List<ModoDisponibilizacaoDTO>>("api/ModosDisponibilizacao");
            return modos ?? new List<ModoDisponibilizacaoDTO>();
        } catch {
            return new List<ModoDisponibilizacaoDTO>();
        }
    }

    public async Task<ProdutoDTO?> CreateProductAsync(ProdutoDTO produto) {
        try {
            await AddAuthToken();
            var response = await _http.PostAsJsonAsync("api/produtos", produto);
            if (response.IsSuccessStatusCode) {
                return await response.Content.ReadFromJsonAsync<ProdutoDTO>();
            }

            return null;
        } catch {
            return null;
        }
    }

    public async Task<ProdutoDTO?> UpdateProductAsync(ProdutoDTO produto) {
        try {
            await AddAuthToken();
            var response = await _http.PutAsJsonAsync($"api/produtos/{produto.Id}", produto);
            if (response.IsSuccessStatusCode) {
                return produto;
            }

            return null;
        } catch {
            return null;
        }
    }

    public async Task<bool> DeleteProductAsync(int id) {
        try {
            await AddAuthToken();
            var response = await _http.DeleteAsync($"api/produtos/{id}");
            return response.IsSuccessStatusCode;
        } catch {
            return false;
        }
    }
}
