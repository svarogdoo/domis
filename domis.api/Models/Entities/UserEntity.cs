using Microsoft.AspNetCore.Identity;

namespace domis.api.Models;

public class UserEntity : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}

