using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyCOLL.Data.Data;
using MyCOLL.API.Repositories;
using MyCOLL.Data.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Vamos adicionar o serviço de acesso à BD
builder.Services.AddDbContext<ApplicationDbContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Registar Repositórios
// using MyCOLL.API.Repositories;
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<ITipoColecionavelRepository, TipoColecionavelRepository>();
builder.Services.AddScoped<IPaisRepository, PaisRepository>();
builder.Services.AddScoped<IModoDisponibilizacaoRepository, ModoDisponibilizacaoRepository>();
builder.Services.AddScoped<IEncomendaRepository, EncomendaRepository>();

//Vamos adicionar o serviço de autenticação
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => {
        options.TokenValidationParameters = new TokenValidationParameters {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                        builder.Configuration["Jwt:Key"]
                    )
                )
        };
    });

// registar serviços adicionais - Autenticação e endpoints da API
builder.Services.AddAuthorization();

// Usar AddIdentityCore para ter UserManager/SignInManager sem criar endpoints /identity/*
// Os endpoints de auth são geridos pelo nosso AuthController com JWT personalizado
builder.Services.AddIdentityCore<ApplicationUser>(options => {
        options.Password.RequireDigit = false;
        options.Password.RequiredLength = 6;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;
    })
    .AddRoles<IdentityRole>()
    .AddSignInManager()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddCors(options => {
    options.AddPolicy("AllowBlazorApp", policy => {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

// Endpoints de Identity removidos - usamos o AuthController para login/register
// app.MapGroup("/identity").MapIdentityApi<ApplicationUser>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//middleware de autenticacao/autorizacao
//a ordem é importante o CORS deve ser o primeiro
//ou pelo menos antes do authentication e authorization

//1. CORS - permitir pedidos do blazor
app.UseCors("AllowBlazorApp");

//2. Authentication - Validar o token JWT e popular User.Identity
app.UseAuthentication();

//2. Authorization - Verificar se o utilizador tem permissão ([Authorize])
app.UseAuthorization();

app.MapControllers();

app.Run();
