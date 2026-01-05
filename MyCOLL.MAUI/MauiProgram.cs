using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using MyCOLL.Shared.Auth;
using MyCOLL.Shared.Services;

namespace MyCOLL.MAUI;

public static class MauiProgram {
    public static MauiApp CreateMauiApp() {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts => { fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular"); });

        builder.Services.AddMauiBlazorWebView();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        builder.Services.AddScoped<ILocalStorageService, LocalStorageService>();
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<IProdutoService, ProdutoService>();
        builder.Services.AddScoped<ICarrinhoService, CarrinhoService>();
        builder.Services.AddScoped<AuthenticationStateProvider, ApiAuthStateProvider>();
        builder.Services.AddTransient<JwtAuthenticationHandler>();
        builder.Services.AddHttpClient("API", client => {
            client.BaseAddress = new Uri("https://localhost:5255/"); // URL da API
        }).AddHttpMessageHandler<JwtAuthenticationHandler>();

        return builder.Build();
    }
}
