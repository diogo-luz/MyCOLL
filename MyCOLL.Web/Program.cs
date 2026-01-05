using Microsoft.AspNetCore.Components.Authorization;
using MyCOLL.Shared.Auth;
using MyCOLL.Shared.Services;
using MyCOLL.Web.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped<ILocalStorageService, LocalStorageService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IProdutoService, ProdutoService>();
builder.Services.AddScoped<ICarrinhoService, CarrinhoService>();
builder.Services.AddScoped<AuthenticationStateProvider, ApiAuthStateProvider>();
builder.Services.AddAuthorizationCore();
builder.Services.AddTransient<JwtAuthenticationHandler>();
builder.Services.AddHttpClient("API", client => {
    client.BaseAddress = new Uri("https://localhost:5255/"); // URL da API
}).AddHttpMessageHandler<JwtAuthenticationHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
