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
        var token = await _localStorage.GetItemAsync<string>("authToken");

        //Se existe um token válido, adicionar ao header Authorization
        if (!string.IsNullOrEmpty(token)) {
            //Formato do header -> Authorization: Bearer <token>
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        // Enviar o pedido (agora com o header Authorization se houver token)
        var response = await base.SendAsync(request, cancellationToken);

        // se receber 401, limpa o token e faz logout
        if (response.StatusCode == HttpStatusCode.Unauthorized) {
            await _localStorage.RemoveItemAsync("authToken");
            // TODO: redirecionar para a página de login
        }

        return response;
    }
}
