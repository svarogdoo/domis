using domis.api.Models;

namespace domis.api.DTOs.User;

public record UserProfileDto(
    string FirstName, 
    string LastName,
    string Email, 
    string? PhoneNumber, 
    CompanyInfo? CompanyInfo, 
    AddressEntity? AddressDelivery, 
    AddressEntity? AddressInvoice
);

public interface IUserProfileDto
{
    string FirstName { get; }
    string LastName { get; }
    string Email { get; }
    string? PhoneNumber { get; }
    CompanyInfo? CompanyInfo { get; }
    AddressEntity? AddressDelivery { get; }
    AddressEntity? AddressInvoice { get; }
}

public record RegularUserProfileDto(
    string FirstName, string LastName,
    string Email, string? PhoneNumber, CompanyInfo? CompanyInfo, AddressEntity? AddressDelivery, AddressEntity? AddressInvoice
) : IUserProfileDto;

public record UserWholesaleProfileDto(
    string FirstName, string LastName,
    string Email, string? PhoneNumber, CompanyInfo? CompanyInfo, AddressEntity? AddressDelivery, AddressEntity? AddressInvoice
) : IUserProfileDto;


//public record UserProfileDto(
//    string FirstName, string LastName, string? AddressLine, 
//    string? City, string? PostalCode, string? Country, string? County, string Email, string? PhoneNumber
//);

//additional field: CompanyName