using domis.api.Models;
using domis.api.Models.Entities;

namespace domis.api.DTOs.User;

public record UserProfileDto(
    string FirstName, 
    string LastName,
    string Email, 
    string? PhoneNumber, 
    CompanyProfileDto? CompanyInfo, 
    AddressProfileDto? AddressDelivery, 
    AddressProfileDto? AddressInvoice
);

public record CompanyProfileDto(
    string? Name,
    string? Number,
    string? FirstName,
    string? LastName
);

public record AddressProfileDto(
    string? AddressLine,
    string? Apartment,
    string? City,
    string? PostalCode,
    string? Country,
    string? County,
    string? ContactPhone,
    string? ContactPerson,
    AddressType? AddressType
);

// public interface IUserProfileDto
// {
//     string FirstName { get; }
//     string LastName { get; }
//     string Email { get; }
//     string? PhoneNumber { get; }
//     CompanyEntity? CompanyInfo { get; }
//     AddressEntity? AddressDelivery { get; }
//     AddressEntity? AddressInvoice { get; }
// }
//
// public record UserProfile(
//     string FirstName, string LastName,
//     string Email, string? PhoneNumber, CompanyEntity? CompanyInfo, AddressEntity? AddressDelivery, AddressEntity? AddressInvoice
// ) : IUserProfileDto;
//
// public record UserWholesaleProfileDto(
//     string FirstName, string LastName,
//     string Email, string? PhoneNumber, CompanyEntity? CompanyInfo, AddressEntity? AddressDelivery, AddressEntity? AddressInvoice
// ) : IUserProfileDto;

//public record UserProfileDto(
//    string FirstName, string LastName, string? AddressLine, 
//    string? City, string? PostalCode, string? Country, string? County, string Email, string? PhoneNumber
//);

//additional field: CompanyName