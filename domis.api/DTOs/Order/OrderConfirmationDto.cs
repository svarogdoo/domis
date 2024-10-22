namespace domis.api.DTOs.Order;

public class OrderConfirmationDto
{
    public int OrderId { get; set; }
    public List<OrderItemWithPriceDto> OrderItems { get; set; } = [];
    public decimal? TotalPrice { get; set; } // Total order price
    public OrderShippingDto? Shipping { get; set; } // Shipping information
}
