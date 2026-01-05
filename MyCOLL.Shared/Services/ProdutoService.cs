using System.Net.Http.Json;
using MyCOLL.Shared.DTOs;

namespace MyCOLL.Shared.Services;

public interface IProdutoService {
    Task<List<ProdutoDTO>> GetAllAsync();
    Task<ProdutoDTO?> GetByIdAsync(int id);
    Task<List<ProdutoDTO>> GetByCategoriaAsync(int categoriaId);
    Task<List<CategoriaDTO>> GetCategoriasAsync();
    Task<List<TipoColecionavelDTO>> GetTiposAsync();
}

public class ProdutoService : IProdutoService {
    private readonly HttpClient _http;

    public ProdutoService(HttpClient http) {
        _http = http;
    }

    public async Task<List<ProdutoDTO>> GetAllAsync() {
        try {
            var produtos = await _http.GetFromJsonAsync<List<ProdutoDTO>>("api/produtos");
            return produtos ?? new List<ProdutoDTO>();
        } catch {
            return new List<ProdutoDTO>();
        }
    }

    public async Task<ProdutoDTO?> GetByIdAsync(int id) {
        try {
            return await _http.GetFromJsonAsync<ProdutoDTO>($"api/produtos/{id}");
        } catch {
            return null;
        }
    }

    public async Task<List<ProdutoDTO>> GetByCategoriaAsync(int categoriaId) {
        try {
            var produtos = await _http.GetFromJsonAsync<List<ProdutoDTO>>($"api/produtos/categoria/{categoriaId}");
            return produtos ?? new List<ProdutoDTO>();
        } catch {
            return new List<ProdutoDTO>();
        }
    }

    public async Task<List<CategoriaDTO>> GetCategoriasAsync() {
        try {
            var categorias = await _http.GetFromJsonAsync<List<CategoriaDTO>>("api/categorias");
            return categorias ?? new List<CategoriaDTO>();
        } catch {
            return new List<CategoriaDTO>();
        }
    }

    public async Task<List<TipoColecionavelDTO>> GetTiposAsync() {
        try {
            var tipos = await _http.GetFromJsonAsync<List<TipoColecionavelDTO>>("api/tipos");
            return tipos ?? new List<TipoColecionavelDTO>();
        } catch {
            return new List<TipoColecionavelDTO>();
        }
    }
}
