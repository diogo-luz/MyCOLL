using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MyCOLL.Data.Data;
using MyCOLL.Data.DTOs;
using MyCOLL.Data.Entities;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace MyCOLL.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase {
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IConfiguration _configuration;

    public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
        IConfiguration configuration) {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO loginData) {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var user = await _userManager.FindByEmailAsync(loginData.Email);
        if (user == null) return Unauthorized(new { message = "Email ou password inválidos!" });

        var result = await _signInManager.CheckPasswordSignInAsync(user, loginData.Password, false);
        if (!result.Succeeded) return Unauthorized(new { message = "Email ou password inválidos!" });

        var token = await GenerateJwtToken(user);
        return Ok(new AuthResponseDTO {
            AccessToken = token,
            ExpiresIn = 3600,
            Email = user.Email ?? string.Empty
        });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDTO registerData) {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        if (registerData.TipoUtilizador != "Cliente" && registerData.TipoUtilizador != "Fornecedor") {
            return BadRequest(new { message = "Tipo de utilizador inválido. Deve ser 'Cliente' ou 'Fornecedor'." });
        }

        var user = new ApplicationUser {
            UserName = registerData.Email,
            Email = registerData.Email,
            Nome = registerData.Nome,
            Apelido = registerData.Apelido,
            TipoUtilizador = registerData.TipoUtilizador
        };

        var result = await _userManager.CreateAsync(user, registerData.Password);
        if (!result.Succeeded) {
            return BadRequest(new { message = "Erro ao criar utilizador", errors = result.Errors });
        }

        await _userManager.AddToRoleAsync(user, registerData.TipoUtilizador);

        return Ok(new { message = "Registo efetuado com sucesso! A aguardar aprovação." });
    }

    private async Task<string> GenerateJwtToken(ApplicationUser user) {
        var jwtKey = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt Key não configurada");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim> {
            new(JwtRegisteredClaimNames.Sub, user.Email ?? string.Empty),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Name, user.UserName ?? string.Empty),
            new(ClaimTypes.Email, user.Email ?? string.Empty)
        };

        var userRoles = await _userManager.GetRolesAsync(user);
        foreach (var role in userRoles) {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
