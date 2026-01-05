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

        // Configurar cultura para Euro
        var culture = new System.Globalization.CultureInfo("pt-PT");
        System.Globalization.CultureInfo.DefaultThreadCurrentCulture = culture;
        System.Globalization.CultureInfo.DefaultThreadCurrentUICulture = culture;

        builder.Services.AddMauiBlazorWebView();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        // 1. HttpClient PRIMEIRO (antes dos serviços que dependem dele)
        builder.Services.AddScoped(_ => new HttpClient {
            BaseAddress = new Uri("https://r1zv1lz2-5255.uks1.devtunnels.ms/")
        });

        // 2. LocalStorage (usado pelo AuthService)
        builder.Services.AddScoped<ILocalStorageService, LocalStorageService>();

        // 3. Serviços que dependem de HttpClient e LocalStorage
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<IProdutoService, ProdutoService>();
        builder.Services.AddScoped<ICarrinhoService, CarrinhoService>();
        builder.Services.AddScoped<IEncomendaService, EncomendaService>();

        // 4. Authorization (necessário para AuthorizeView funcionar)
        builder.Services.AddAuthorizationCore();
        builder.Services.AddScoped<AuthenticationStateProvider, ApiAuthStateProvider>();

        return builder.Build();
    }
}
