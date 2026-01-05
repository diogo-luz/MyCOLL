using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using MyCOLL.Shared.Services;

namespace MyCOLL.Shared.Auth;

/// <summary>
/// AuthenticationStateProvider customizado que lê o token JWT do LocalStorage
/// e fornece o estado de autenticação ao Blazor.
/// </summary>
public class ApiAuthStateProvider : AuthenticationStateProvider {
    private readonly ILocalStorageService _localStorage;

    public ApiAuthStateProvider(ILocalStorageService localStorage) {
        _localStorage = localStorage;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync() {
        var token = await _localStorage.GetItemAsync<string>("authToken");

        if (string.IsNullOrEmpty(token)) {
            // Utilizador não autenticado - retornar ClaimsPrincipal anónimo
            var anonymous = new ClaimsPrincipal(new ClaimsIdentity());
            return new AuthenticationState(anonymous);
        }

        try {
            // Descodificar o token JWT e extrair claims
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            // Criar lista de claims a partir do token
            var claims = new List<Claim>();

            // Extrair claims standard do JWT
            foreach (var claim in jwtToken.Claims) {
                // Mapear claims JWT para claims .NET standard
                var claimType = claim.Type switch {
                    JwtRegisteredClaimNames.Sub => ClaimTypes.NameIdentifier,
                    JwtRegisteredClaimNames.Email => ClaimTypes.Email,
                    JwtRegisteredClaimNames.Name => ClaimTypes.Name,
                    "role" => ClaimTypes.Role,
                    _ => claim.Type
                };

                claims.Add(new Claim(claimType, claim.Value));
            }

            // Criar identity autenticada
            var identity = new ClaimsIdentity(claims, "jwt");
            var user = new ClaimsPrincipal(identity);

            return new AuthenticationState(user);
        } catch {
            // Token inválido - retornar anónimo
            var anonymous = new ClaimsPrincipal(new ClaimsIdentity());
            return new AuthenticationState(anonymous);
        }
    }

    /// <summary>
    /// Notifica o Blazor que o estado de autenticação mudou (após login ou logout).
    /// Deve ser chamado após login ou logout bem-sucedido.
    /// </summary>
    public void NotifyUserAuthentication() {
        var authState = GetAuthenticationStateAsync();
        NotifyAuthenticationStateChanged(authState);
    }
}
