using System.Text.Json;
using Microsoft.JSInterop;

namespace MyCOLL.Shared.Services;

public interface ILocalStorageService {
    Task SetItemAsync<T>(string key, T value);
    Task<T?> GetItemAsync<T>(string key);
    Task RemoveItemAsync(string key);
    Task ClearAsync();
}

public class LocalStorageService : ILocalStorageService {
    private readonly IJSRuntime _js;

    public LocalStorageService(IJSRuntime js) {
        _js = js;
    }

    public async Task SetItemAsync<T>(string key, T value) {
        try {
            var json = JsonSerializer.Serialize(value);
            await _js.InvokeVoidAsync("localStorage.setItem", key, json);
        } catch {
            // Ignora se falhar no servidor
        }
    }

    public async Task<T?> GetItemAsync<T>(string key) {
        try {
            var json = await _js.InvokeAsync<string?>("localStorage.getItem", key);
            if (string.IsNullOrEmpty(json)) return default;

            // Se estamos a pedir uma STRING, tentamos devolver direto
            // Isso resolve o problema se o token foi gravado sem aspas de JSON
            if (typeof(T) == typeof(string) && !json.StartsWith("\"")) {
                return (T)(object)json;
            }

            return JsonSerializer.Deserialize<T>(json);
        } catch (JsonException) {
            if (typeof(T) == typeof(string)) {
                var raw = await _js.InvokeAsync<string?>("localStorage.getItem", key);
                return (T)(object)raw!;
            }

            Console.WriteLine($"Erro JSON ao ler {key}");
            return default;
        } catch (Exception ex) {
            Console.WriteLine($"Erro fatal LocalStorage ({key}): {ex.Message}");
            return default;
        }
    }

    public async Task RemoveItemAsync(string key) {
        await _js.InvokeVoidAsync("localStorage.removeItem", key);
    }

    public async Task ClearAsync() {
        await _js.InvokeVoidAsync("localStorage.clear");
    }
}
