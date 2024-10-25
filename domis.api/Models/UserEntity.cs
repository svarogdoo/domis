using Microsoft.AspNetCore.Identity;

namespace domis.api.Models;

public class UserEntity : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? CompanyName { get; set; }
    public string? Country { get; set; }
    public string? County { get; set; }
    public string? City { get; set; }
    public string? AddressLine { get; set; }
    public string? Apartment { get; set; }
    public string? PostalCode { get; set; }
    public string? Address { get; set; } //TODO: check if used anywhere
}