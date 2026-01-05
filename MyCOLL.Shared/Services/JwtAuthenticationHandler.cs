using System.Net;
using System.Net.Http.Headers;

namespace MyCOLL.Shared.Services;

/// <summary>
/// Injetar automaticamente o token JWT em todos os pedidos HTTP
/// </summary>
public class JwtAuthenticationHandler : DelegatingHandler {
    private readonly ILocalStorageService _localStorage;

    public JwtAuthenticationHandler(ILocalStorageService localStorage) {
        _localStorage = localStorage;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken) {
        try {
            var token = await _localStorage.GetItemAsync<string>("authToken");

            if (!string.IsNullOrEmpty(token)) {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        } catch {
            // Se o JS interop falhar, deixamos o pedido ir sem token
        }

        var response = await base.SendAsync(request, cancellationToken);

        if (response.StatusCode == HttpStatusCode.Unauthorized) {
            await _localStorage.RemoveItemAsync("authToken");
        }

        return response;
    }
}