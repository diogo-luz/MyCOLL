using System.Net.Http.Json;
using MyCOLL.Shared.DTOs;

namespace MyCOLL.Shared.Services;

public interface IAuthService {
    Task<AuthResult> LoginAsync(string email, string password);
    Task<AuthResult> RegisterAsync(RegisterDTO registerData);
    Task LogoutAsync();
    Task<string?> GetTokenAsync();
    Task<bool> IsAuthenticatedAsync();
}

public class AuthService : IAuthService {
    private readonly HttpClient _http;
    private readonly ILocalStorageService _localStorage;

    public AuthService(HttpClient http, ILocalStorageService localStorage) {
        _http = http;
        _localStorage = localStorage;
    }

    public async Task<AuthResult> LoginAsync(string email, string password) {
        try {
            var response = await _http.PostAsJsonAsync("api/auth/login", new LoginDTO {
                Email = email,
                Password = password
            });

            if (response.IsSuccessStatusCode) {
                var authResponse = await response.Content.ReadFromJsonAsync<AuthResponseDTO>();
                if (authResponse != null) {
                    await _localStorage.SetItemAsync("authToken", authResponse.AccessToken);
                    return new AuthResult { Success = true };
                }
            }

            var error = await response.Content.ReadAsStringAsync();
            return new AuthResult { Success = false, Error = error };
        } catch (Exception ex) {
            return new AuthResult { Success = false, Error = ex.Message };
        }
    }

    public async Task<AuthResult> RegisterAsync(RegisterDTO registerData) {
        try {
            var response = await _http.PostAsJsonAsync("api/auth/register", registerData);

            if (response.IsSuccessStatusCode) {
                return new AuthResult { Success = true };
            }

            var error = await response.Content.ReadAsStringAsync();
            return new AuthResult { Success = false, Error = error };
        } catch (Exception ex) {
            return new AuthResult { Success = false, Error = ex.Message };
        }
    }

    public async Task LogoutAsync() {
        await _localStorage.RemoveItemAsync("authToken");
    }

    public async Task<string?> GetTokenAsync() {
        return await _localStorage.GetItemAsync<string>("authToken");
    }

    public async Task<bool> IsAuthenticatedAsync() {
        var token = await GetTokenAsync();
        return !string.IsNullOrEmpty(token);
    }
}

public class AuthResult {
    public bool Success { get; set; }
    public string? Error { get; set; }
}
