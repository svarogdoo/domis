using Microsoft.AspNetCore.Identity;
using domis.api.Models;

namespace domis.api.Database;

public static class SeedData
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

        await SeedRolesAsync(roleManager);

        await SeedAdminUserAsync(userManager, roleManager);
    }

    private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
    {
        string[] roles = Enum.GetValues(typeof(Roles)).Cast<Roles>().Select(r => r.GetRoleName()).ToArray();

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }

    private static async Task SeedAdminUserAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        var adminEmail = "admin@mail.com";
        var adminPassword = "sifra";

        var adminUser = await userManager.FindByEmailAsync(adminEmail);

        if (adminUser == null)
        {
            adminUser = new User
            {
                UserName = adminEmail,
                Email = adminEmail
            };

            var result = await userManager.CreateAsync(adminUser, adminPassword);

            if (result.Succeeded)
            {
                if (await roleManager.RoleExistsAsync(Roles.Admin.GetRoleName()))
                {
                    await userManager.AddToRoleAsync(adminUser, Roles.Admin.GetRoleName());
                }
            }
        }
    }
}
