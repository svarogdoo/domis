using Microsoft.AspNetCore.Identity;

namespace domis.api.Models;

public class UserEntity : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public CompanyInfo? CompanyInfo { get; set; } //TODO: Maybe remove?
}

public class CompanyInfo
{
    public required int Id { get; set; }
    public string? Name { get; set; }
    public long? Number { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}

