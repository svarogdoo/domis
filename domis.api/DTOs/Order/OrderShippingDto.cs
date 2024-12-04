namespace domis.api.DTOs.Order;

public class OrderShippingDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int CountryId { get; set; }
    public string City { get; set; }
    public string Address { get; set; }
    public string? Apartment { get; set; }
    public string? County { get; set; }
    public string PostalCode { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    
    //company info
    public string? CompanyName { get; set; }
    public long? CompanyNumber { get; set; }
    public string? CompanyFirstName { get; set; }
    public string? CompanyLastName { get; set; }
    
    //delivery address optional fields
    public string? ContactPhone { get; set; }
    public string? ContactPerson { get; set; }
    
    public string AddressType { get; set; }
}

public class OrderShippingWithCountryDto : OrderShippingDto
{
    public string CountryName { get; set; } // Added country name
}

[Obsolete("do not use", true)]
public class OrderShippingDto2
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? CompanyName { get; set; }
    public int CountryId { get; set; }
    public string City { get; set; }
    public string Address { get; set; }
    public string? Apartment { get; set; }
    public string? County { get; set; }
    public string PostalCode { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
}

[Obsolete("do not use", true)]
public class OrderShippingWithCountryDto2 : OrderShippingDto2
{
    public string CountryName { get; set; } // Added country name
}