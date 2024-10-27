using domis.api.Models;

namespace domis.api.DTOs.User;
public class UserWithRolesDto
{
    public string UserId { get; set; }
    public string UserName { get; set; }
    public List<string> Roles { get; set; } = [];
}

public class UserWithRoles
{
    public UserEntity User { get; set; }
    public List<string> Roles { get; set; }
}