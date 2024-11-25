using Microsoft.AspNetCore.Identity;

namespace domis.api.Models;

public class UserEntity : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Country { get; set; }
    public string? County { get; set; }
    public string? City { get; set; }
    public string? AddressLine { get; set; }
    public string? Apartment { get; set; }
    public string? PostalCode { get; set; }
    public CompanyInfo? CompanyInfo { get; set; } 
}

public class CompanyInfo
{
    public required int Id { get; set; }
    public string? Name { get; set; }
    public long? Number { get; set; }
}