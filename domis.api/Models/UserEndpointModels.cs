namespace domis.api.Models;

public record RegisterRequest
(
    string FirstName,
    string LastName,
    string Email,
    string Password,
    int? CartId
);

public record LoginRequest (
    string Email,
    string Password,
    string TwoFactorCode,
    string TwoFactorRecoveryCode,
    int? CartId
);

public record ProfileUpdateRequest(
    string? FirstName,
    string? LastName,
    string? AddressLine,
    string? Apartment,
    string? City,
    string? PostalCode,
    string? Country,
    string? County,
    string? PhoneNumber,
    CompanyInfo? CompanyInfo
);

public record LoginWithCartRequest(
    string Email,
    string Password,
    int? CartId
);

//public record AddressUpdateRequest(
//    string AddressLine,
//    string City,
//    string ZipCode,
//    string Country
//);