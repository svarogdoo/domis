namespace domis.api.DTOs.User;

public record UserProfileDto(
    string FirstName, string LastName, string? AddressLine, 
    string? City, string? ZipCode, string? Country, string Email, string? PhoneNumber
);