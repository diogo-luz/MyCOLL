using Microsoft.AspNetCore.Components.Authorization;
using MyCOLL.Shared.Auth;
using MyCOLL.Shared.Services;
using MyCOLL.Web.Components;

var builder = WebApplication.CreateBuilder(args);

// Aumentar o limite do SignalR para aguentar imagens Base64
builder.Services.AddSignalR(e => {
    e.MaximumReceiveMessageSize = 10 * 1024 * 1024; // Aumentar para 10MB
});

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped<ILocalStorageService, LocalStorageService>();
builder.Services.AddScoped<AuthenticationStateProvider, ApiAuthStateProvider>();
builder.Services.AddAuthentication();
builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();

builder.Services.AddTransient<JwtAuthenticationHandler>();

var apiUrl = new Uri("http://localhost:5255/");

// Registar serviços como Typed Clients para injetar HttpClient configurado
builder.Services.AddHttpClient<IAuthService, AuthService>(client =>
        client.BaseAddress = apiUrl)
    .AddHttpMessageHandler<JwtAuthenticationHandler>();

builder.Services.AddHttpClient<IProdutoService, ProdutoService>(client =>
        client.BaseAddress = apiUrl)
    .AddHttpMessageHandler<JwtAuthenticationHandler>();

builder.Services.AddHttpClient<ICarrinhoService, CarrinhoService>(client =>
        client.BaseAddress = apiUrl)
    .AddHttpMessageHandler<JwtAuthenticationHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddAdditionalAssemblies(typeof(MyCOLL.Shared.Pages.Home).Assembly);

app.Run();
