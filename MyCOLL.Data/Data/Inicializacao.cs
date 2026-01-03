using Microsoft.AspNetCore.Identity;

namespace MyCOLL.Data.Data;

public class Inicializacao {
    public static async Task CriaDadosIniciais(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager) {

        // Adicionar default roles
        string[] roles = ["Admin", "Funcionario", "Fornecedor", "Cliente"];

        foreach (var role in roles) {
            if (!await roleManager.RoleExistsAsync(role)) {
                IdentityRole roleRole = new IdentityRole(role);
                await roleManager.CreateAsync(roleRole);
            }
        }

        // Adicionar default user - Admin
        var defaultAdmin = new ApplicationUser {
            UserName = "admin@mycoll.com",
            Email = "admin@mycoll.com",
            Nome = "Administrador",
            Apelido = "Sistema",
            TipoUtilizador = "Admin",
            Activo = true,
            EmailConfirmed = true,
            PhoneNumberConfirmed = true,
        };

        if (userManager.Users.All(u => u.Id != defaultAdmin.Id)) {
            var user = await userManager.FindByEmailAsync(defaultAdmin.Email);
            if (user == null) {
                await userManager.CreateAsync(defaultAdmin, "Is3C..00");
                await userManager.AddToRoleAsync(defaultAdmin, "Admin");
            }
        }

        // Garantir que admin tem role Admin
        var defAdmin = await userManager.FindByEmailAsync("admin@mycoll.com");
        if (defAdmin != null) {
            if (!await userManager.IsInRoleAsync(defAdmin, "Admin")) {
                await userManager.AddToRoleAsync(defAdmin, "Admin");
            }
        }

        // Adicionar default user - Funcionario
        var defaultFuncionario = new ApplicationUser {
            UserName = "funcionario@mycoll.com",
            Email = "funcionario@mycoll.com",
            Nome = "Funcionário",
            Apelido = "Teste",
            TipoUtilizador = "Funcionario",
            Activo = true,
            EmailConfirmed = true,
            PhoneNumberConfirmed = true,
        };

        if (await userManager.FindByEmailAsync(defaultFuncionario.Email) == null) {
            await userManager.CreateAsync(defaultFuncionario, "Is3C..00");
            await userManager.AddToRoleAsync(defaultFuncionario, "Funcionario");
        }
    }
}
