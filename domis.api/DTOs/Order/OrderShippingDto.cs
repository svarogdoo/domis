using domis.api.DTOs.Common;

namespace domis.api.DTOs.Order;

public class OrderShippingDto
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string CompanyName { get; set; }
    public int CountryId { get; set; }
    public string City { get; set; }
    public string Address { get; set; }
    public string Apartment { get; set; }
    public string County { get; set; }
    public string PostalCode { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public CountryDto CountryDto { get; set; }
}