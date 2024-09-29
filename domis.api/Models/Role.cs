using Microsoft.AspNetCore.Identity;

namespace domis.api.Models;

public sealed class Role : IdentityRole
{
    // Parameterless constructor needed by Entity Framework
    public Role() : base()
    {
    }

    // Constructor to initialize Role with a role name (optional)
    public Role(string roleName) : base(roleName)
    {
    }
    
    public decimal? Discount { get; set; }
}