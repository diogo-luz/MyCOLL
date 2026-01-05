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

builder.Services.AddIdentityApiEndpoints<ApplicationUser>()
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

//mapear uma rota para o endpoint de identity
app.MapGroup("/identity").MapIdentityApi<ApplicationUser>();

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
