using domis.api.DTOs.User;
using domis.api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace domis.api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AdminController : ControllerBase
{
    private readonly UserManager<UserEntity> _userManager;
    private readonly RoleManager<Role> _roleManager;

    public AdminController(UserManager<UserEntity> userManager, RoleManager<Role> roleManager)
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

    [HttpGet("users")]
    // [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _userManager.Users.ToListAsync(); // Fetch all users
        var usersWithRoles = new List<UserWithRolesDto>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user); // Get roles for each user
            usersWithRoles.Add(new UserWithRolesDto
            {
                UserId = user.Id,
                UserName = user.UserName,
                Roles = roles.ToList()
            });
        }

        return Ok(usersWithRoles); // Return the list of users with their roles
    }


    [HttpGet("user/{userId}")]
  //  [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> GetUser([FromRoute] string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
      
        if (user == null)
        {
            return NotFound("User not found.");
        }

        // Remove existing roles if necessary
        var userRoles = await _userManager.GetRolesAsync(user);

        return Ok(new UserWithRoles() {User = user, Roles = [.. userRoles] });
    }
    
    [HttpGet("roles")]
   // [Authorize(Roles = "Administrator")]
    public Task<IActionResult> GetRoles()
    {
        var roles =  _roleManager.Roles;
        
        return Task.FromResult<IActionResult>(Ok(roles));
    }
   
    [HttpPost("roles/{roleId}")]
    // [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> UpdateRole([FromRoute] string roleId, [FromBody] DiscountRequest request)
    {
        var role = await _roleManager.FindByIdAsync(roleId);
        
        if (role is null)
        {
            return BadRequest($"Role with id: {roleId} does not exist.");
        }
        
        role.Discount = request.Discount;

        var result = await _roleManager.UpdateAsync(role);
     
        if (result.Succeeded)
        {
            return Ok(role);
        }

        return BadRequest($"Failed to update role with id: {roleId}.");    
    }
   
    [HttpPost("user-role/{userId}")]
    // [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> AddRole([FromRoute] string userId, [FromBody] RoleRequest request)
    {
        var role = request.Role.GetRoleName();
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound("User not found.");
        }

        if (!await _roleManager.RoleExistsAsync(role))
        {
            return BadRequest($"{role} role does not exist.");
        }

        // Remove existing roles if necessary
        var userRoles = await _userManager.GetRolesAsync(user);
      
        if (userRoles.Contains(role))
        {
            return Ok($"User already has been promoted to {role} successfully.");
        }
        
        // Add the role
        var result = await _userManager.AddToRoleAsync(user, role);

        if (result.Succeeded)
        {
            return Ok($"User promoted to {role} successfully.");
        }

        return BadRequest($"Failed to promote user to {role}.");
    }
    
    [HttpDelete("user-role/{userId}")]
    // [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> DeleteRole([FromRoute] string userId, [FromBody] RoleRequest request)
    {
        var role = request.Role.GetRoleName();

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound("User not found.");
        }

        if (!await _roleManager.RoleExistsAsync(role))
        {
            return BadRequest($"{role} role does not exist.");
        }

        // Remove existing roles if necessary
        var userRoles = await _userManager.GetRolesAsync(user);
        if (userRoles.Contains(role))
        {
           var result = await _userManager.RemoveFromRoleAsync(user, Roles.User.GetRoleName());
           
           if (result.Succeeded)
           {
               return Ok($"User has no longer {role} role");
           }

           return BadRequest($"Failed to delete {role} role for user.");
        }

        return BadRequest($"User {userId} does not have role {role}");
      
    }

    public record RoleRequest(Roles Role);
    public record DiscountRequest(decimal Discount);

}