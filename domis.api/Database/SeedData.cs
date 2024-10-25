using domis.api.Models;
using Microsoft.AspNetCore.Identity;

namespace domis.api.Database;

public static class SeedData
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<UserEntity>>();

        await SeedRolesAsync(roleManager);

        await SeedAdminUserAsync(userManager, roleManager);
    }

    private static async Task SeedRolesAsync(RoleManager<Role> roleManager)
    {
        string[] roles = Enum.GetValues(typeof(Roles)).Cast<Roles>().Select(r => r.RoleName()).ToArray();

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new Role(role));
            }
        }
    }

    private static async Task SeedAdminUserAsync(UserManager<UserEntity> userManager, RoleManager<Role> roleManager)
    {
        var adminEmail = "admin@mail.com";
        var adminPassword = "sifra";

        var adminUser = await userManager.FindByEmailAsync(adminEmail);

        if (adminUser == null)
        {
            adminUser = new UserEntity
            {
                UserName = adminEmail,
                Email = adminEmail,
                FirstName = "AdminFirstName",
                LastName = "AdminLastName",
            };

            var result = await userManager.CreateAsync(adminUser, adminPassword);

            if (result.Succeeded)
            {
                if (await roleManager.RoleExistsAsync(Roles.Admin.RoleName()))
                {
                    await userManager.AddToRoleAsync(adminUser, Roles.Admin.RoleName());
                }
            }
        }
    }
}