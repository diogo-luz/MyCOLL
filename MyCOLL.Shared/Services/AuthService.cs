using System.Net.Http.Json;
using MyCOLL.Shared.DTOs;

namespace MyCOLL.Shared.Services;

public interface IAuthService {
    Task<AuthResult> LoginAsync(string email, string password);
    Task<AuthResult> RegisterAsync(RegisterDTO registerData);
    Task LogoutAsync();
    Task<string?> GetTokenAsync();
    Task<bool> IsAuthenticatedAsync();

    // New methods
    Task<UserProfileDTO?> GetProfileAsync();
    Task<AuthResult> UpdateProfileAsync(UserProfileDTO profile);
    Task<AuthResult> ChangePasswordAsync(ChangePasswordDTO data);
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

    public async Task<UserProfileDTO?> GetProfileAsync() {
        await AddAuthToken();
        try {
            return await _http.GetFromJsonAsync<UserProfileDTO>("api/auth/profile");
        } catch {
            return null;
        }
    }

    public async Task<AuthResult> UpdateProfileAsync(UserProfileDTO profile) {
        await AddAuthToken();
        try {
            var response = await _http.PutAsJsonAsync("api/auth/profile", profile);
            if (response.IsSuccessStatusCode) {
                return new AuthResult { Success = true };
            }

            var error = await ParseErrorResponse(response);
            return new AuthResult { Success = false, Error = error };
        } catch (Exception ex) {
            return new AuthResult { Success = false, Error = ex.Message };
        }
    }

    public async Task<AuthResult> ChangePasswordAsync(ChangePasswordDTO data) {
        await AddAuthToken();
        try {
            var response = await _http.PostAsJsonAsync("api/auth/change-password", data);
            if (response.IsSuccessStatusCode) {
                return new AuthResult { Success = true };
            }

            var error = await ParseErrorResponse(response);
            return new AuthResult { Success = false, Error = error };
        } catch (Exception ex) {
            return new AuthResult { Success = false, Error = ex.Message };
        }
    }

    private async Task AddAuthToken() {
        var token = await GetTokenAsync();
        if (!string.IsNullOrEmpty(token)) {
            _http.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }
    }

    private async Task<string> ParseErrorResponse(HttpResponseMessage response) {
        var content = await response.Content.ReadAsStringAsync();
        try {
            // Tentar extrair a propriedade "message" do JSON
            var options = new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var errorObj = System.Text.Json.JsonSerializer.Deserialize<ErrorResponse>(content, options);
            if (!string.IsNullOrEmpty(errorObj?.Message)) {
                return errorObj.Message;
            }
        } catch {
            // Se falhar o parse, retorna o conteúdo original
        }

        return content;
    }

    private class ErrorResponse {
        public string? Message { get; set; }
    }
}

public class AuthResult {
    public bool Success { get; set; }
    public string? Error { get; set; }
}
