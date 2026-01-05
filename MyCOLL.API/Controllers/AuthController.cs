using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MyCOLL.Data.Data;
using MyCOLL.Shared.DTOs;
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

    [Authorize]
    [HttpGet("profile")]
    public async Task<ActionResult<UserProfileDTO>> GetProfile() {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _userManager.FindByIdAsync(userId!);

        if (user == null) return NotFound("Utilizador não encontrado");

        return Ok(new UserProfileDTO {
            Id = user.Id,
            Nome = user.Nome,
            Apelido = user.Apelido,
            Email = user.Email!,
            NIF = user.NIF,
            Rua = user.Rua,
            Localidade = user.Localidade,
            CodigoPostal = user.CodigoPostal,
            Pais = user.Pais,
            Telefone = user.PhoneNumber,
            Fotografia = user.Fotografia,
            TipoUtilizador = user.TipoUtilizador
        });
    }

    [Authorize]
    [HttpPut("profile")]
    public async Task<IActionResult> UpdateProfile([FromBody] UserProfileDTO profile) {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _userManager.FindByIdAsync(userId!);

        if (user == null) return NotFound("Utilizador não encontrado");

        user.Nome = profile.Nome;
        user.Apelido = profile.Apelido;
        user.NIF = profile.NIF;
        user.Rua = profile.Rua;
        user.Localidade = profile.Localidade;
        user.CodigoPostal = profile.CodigoPostal;
        user.Pais = profile.Pais;
        user.PhoneNumber = profile.Telefone;

        // Se a foto vier, atualiza. (Lógica simples, poderia ser melhorada)
        if (profile.Fotografia != null) {
            user.Fotografia = profile.Fotografia;
        }

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded) {
            return BadRequest(new { message = "Erro ao atualizar perfil", errors = result.Errors });
        }

        return Ok(new { message = "Perfil atualizado com sucesso" });
    }

    [Authorize]
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO data) {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _userManager.FindByIdAsync(userId!);

        if (user == null) return NotFound("Utilizador não encontrado");

        var result = await _userManager.ChangePasswordAsync(user, data.CurrentPassword, data.NewPassword);

        if (!result.Succeeded) {
            return BadRequest(new {
                message = "Erro ao alterar a password. Verifique se a password atual está correta.",
                errors = result.Errors
            });
        }

        return Ok(new { message = "Password alterada com sucesso" });
    }

    private async Task<string> GenerateJwtToken(ApplicationUser user) {
        var jwtKey = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt Key não configurada");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim> {
            new(JwtRegisteredClaimNames.Sub, user.Id), // FIX: Sub deve ser o ID para o mapeamento automático funcionar
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
