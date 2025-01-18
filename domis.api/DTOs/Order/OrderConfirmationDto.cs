namespace domis.api.DTOs.Order;

public class OrderConfirmationDto
{
    public int OrderId { get; init; }
    public List<OrderItemWithPriceDto> OrderItems { get; init; } = [];
    public decimal? TotalPrice { get; init; }
    public OrderShippingDto? InvoiceAddress { get; set; }
    public OrderShippingDto? DeliveryAddress { get; set; }
    public DateTime CreatedAt { get; init; }
}