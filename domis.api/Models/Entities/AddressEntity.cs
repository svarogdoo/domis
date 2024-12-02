namespace domis.api.Models;

public enum AddressType
{
    Unknown,
    Delivery,
    Invoice
}

public class AddressEntity
{
    public int Id { get; set; }
    public string? UserId { get; set; }

    public string? Country { get; set; }
    public string? County { get; set; }
    public string? City { get; set; }
    public string? AddressLine { get; set; }
    public string? Apartment { get; set; }
    public string? PostalCode { get; set; }

    public string? ContactPerson { get; set; }
    public string? ContactPhone { get; set; }
    
    public AddressType AddressType { get; set; }
}