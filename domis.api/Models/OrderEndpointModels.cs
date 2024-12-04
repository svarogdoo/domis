using domis.api.DTOs.User;
using domis.api.Models.Entities;

namespace domis.api.Models;

public record UpdateOrderShippingResponse(bool Updated);
public record DeleteOrderShippingResponse(bool Deleted);
public record CreateOrderShippingRequest(
    AddressShippingDto AddressInvoice,
    AddressShippingDto? AddressDelivery,
    CompanyProfileDto? CompanyInfo
);

public record AddressShippingDto(
    string FirstName,
    string LastName,
    string PhoneNumber,
    string Email,
    string AddressLine,
    string? Apartment,
    string City,
    string PostalCode,
    string Country,
    string? County,
    string? ContactPhone,
    string? ContactPerson,
    AddressType? AddressType
);

public record CreateOrderRequest(int CartId, int PaymentStatusId, int InvoiceOrderShippingId, int? DeliveryOrderShippingId, int PaymentVendorTypeId, string Comment);
public record CreateOrderResponse(int OrderId);

public record UpdateOrderStatusRequest(int OrderId, int StatusId);
public record UpdateOrderResponse(bool Updated);