using domis.api.DTOs.Order;

public class OrderDetailsDto
{
    public int OrderId { get; set; }
    public string? UserId { get; set; }
    public decimal Amount { get; set; }
    public string? Comment { get; set; }
    public DateTime OrderCreatedAt { get; set; }
    public OrderStatusdetialsDto OrderStatus { get; set; }
    public OrderShippingDetailsDto? OrderShipping { get; set; }
    public PaymentDetailsDto PaymentDetails { get; set; }
    public List<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();
}

public class OrderStatusdetialsDto
{
    public int OrderStatusId { get; set; }
    public string OrderStatusName { get; set; }
}
public class OrderShippingDetailsDto
{
    public int OrderShippingId { get; set; }
    public string ShippingFirstName { get; set; }
    public string ShippingLastName { get; set; }
    public string? ShippingCompanyName { get; set; }
    public int ShippingCountryId { get; set; }
    public string ShippingCountryName { get; set; }
    public string ShippingCity { get; set; }
    public string ShippingAddress { get; set; }
    public string? ShippingApartment { get; set; }
    public string? ShippingCounty { get; set; }
    public string ShippingPostalCode { get; set; }
    public string ShippingPhoneNumber { get; set; }
    public string ShippingEmail { get; set; }
}

public class PaymentDetailsDto
{
    public int? PaymentStatusId { get; set; }
    public string? PaymentStatusName { get; set; }
    public int? PaymentVendorTypeId { get; set; }
    public string? PaymentVendorTypeName { get; set; }
}

public class OrderItemDto
{
    public int OrderItemId { get; set; }
    public int ProductId { get; set; }
    public decimal OrderItemQuantity { get; set; }
    public DateTime OrderItemCreatedAt { get; set; }
    public DateTime? OrderItemModifiedAt { get; set; }
    public ProductOrderDetailsDto ProductDetails { get; set; }
}

public class ProductOrderDetailsDto
{
    public string? ProductName { get; set; }
    public string? ProductDescription { get; set; }
    public string? Url { get; set; }
}
