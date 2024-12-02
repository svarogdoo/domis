using domis.api.Models;
using domis.api.Models.Entities;

namespace domis.api.DTOs.User;

public class UserWithRolesDto
{
    public required string UserId { get; set; }
    public required string UserName { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string Role { get; set; }
    public List<string> Roles { get; set; } = [];
}

public class UserWithRoles
{
    public UserEntity User { get; set; }
    public List<string> Roles { get; set; }
}