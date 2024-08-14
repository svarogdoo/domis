using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using domis.api.Models;

namespace domis.api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AdminController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AdminController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    [HttpPost("promote-to-admin")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> PromoteToAdmin([FromBody] string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound("User not found.");
        }

        if (!await _roleManager.RoleExistsAsync(Roles.Admin.GetRoleName()))
        {
            return BadRequest("Admin role does not exist.");
        }

        // Remove existing roles if necessary
        var userRoles = await _userManager.GetRolesAsync(user);
        if (userRoles.Contains(Roles.User.GetRoleName()))
        {
            await _userManager.RemoveFromRoleAsync(user, Roles.User.GetRoleName());
        }

        // Add the admin role
        var result = await _userManager.AddToRoleAsync(user, Roles.Admin.GetRoleName());

        if (result.Succeeded)
        {
            return Ok("User promoted to admin successfully.");
        }

        return BadRequest("Failed to promote user to admin.");
    }
}