namespace domis.api.Models;

public record SalesPoint
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Address { get; init; } = string.Empty;
    public string[] PhoneNumbers { get; init; } = [];
    public string WorkingHours { get; init; } = string.Empty;
    public string Image { get; init; } = string.Empty;
    public string GoogleMapPin { get; init; } = string.Empty;
    public string? OptionalInfo { get; init; }
}
