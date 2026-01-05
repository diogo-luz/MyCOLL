using System.Net.Http.Json;
using MyCOLL.Shared.DTOs;

namespace MyCOLL.Shared.Services;

public interface IEncomendaService {
    Task<List<EncomendaDTO>> GetMinhasEncomendasAsync();
    Task<SupplierDashboardDTO?> GetSupplierDashboardAsync();
}

public class EncomendaService : IEncomendaService {
    private readonly HttpClient _http;
    private readonly ILocalStorageService _localStorage;

    public EncomendaService(HttpClient http, ILocalStorageService localStorage) {
        _http = http;
        _localStorage = localStorage;
    }

    public async Task<List<EncomendaDTO>> GetMinhasEncomendasAsync() {
        try {
            // Anexar token manualmente
            var token = await _localStorage.GetItemAsync<string>("authToken");
            if (!string.IsNullOrEmpty(token)) {
                _http.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }

            return await _http.GetFromJsonAsync<List<EncomendaDTO>>("api/encomendas/minhas")
                   ?? new List<EncomendaDTO>();
        } catch {
            return new List<EncomendaDTO>();
        }
    }

    public async Task<SupplierDashboardDTO?> GetSupplierDashboardAsync() {
        try {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            if (!string.IsNullOrEmpty(token)) {
                _http.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }

            return await _http.GetFromJsonAsync<SupplierDashboardDTO>("api/encomendas/vendas");
        } catch {
            return null;
        }
    }
}
