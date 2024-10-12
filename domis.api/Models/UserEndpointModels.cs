namespace domis.api.Models;

public record RegisterRequest
(
    string FirstName,
    string LastName,
    string Email,
    string Password
);

public record ProfileUpdateRequest(
    string? FirstName,
    string? LastName,
    string? AddressLine,
    string? City,
    string? ZipCode,
    string? Country,
    string? PhoneNumber
);