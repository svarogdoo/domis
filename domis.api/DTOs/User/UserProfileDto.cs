using domis.api.Models;

namespace domis.api.DTOs.User;

public interface IUserProfileDto
{
    string FirstName { get; }
    string LastName { get; }
    string Email { get; }
    string? PhoneNumber { get; }
    CompanyInfo? CompanyInfo { get; }
}

public record UserProfileDto(
    string FirstName, string LastName,
    string Email, string? PhoneNumber, CompanyInfo? CompanyInfo
) : IUserProfileDto;

public record UserWholesaleProfileDto(
    string FirstName, string LastName,
    string Email, string? PhoneNumber, CompanyInfo? CompanyInfo
) : IUserProfileDto;


//public record UserProfileDto(
//    string FirstName, string LastName, string? AddressLine, 
//    string? City, string? PostalCode, string? Country, string? County, string Email, string? PhoneNumber
//);

//additional field: CompanyName