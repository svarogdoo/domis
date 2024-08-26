using domis.api.DTOs.Product;

namespace domis.api.DTOs.Order;

public class OrderDto
{
    public int Id { get; set; }
    public int? UserId { get; set; }
    public int StatusId { get; set; }
    public int? OrderShippingId { get; set; }
    public int? PaymentStatusId { get; set; }
    public int? PaymentVendorTypeId { get; set; }
    public decimal? PaymentAmount { get; set; }
    public string Comment { get; set; }
    public DateTime CreatedAt { get; set; }
    public OrderShippingDto OrderShippingDto { get; set; }
    public PaymentStatusDto PaymentStatusDto { get; set; }
    public OrderStatusDto OrderStatusDto { get; set; }
    public PaymentVendorTypeDto PaymentVendorTypeDto { get; set; }
    public List<OrderItemDto> OrderItems { get; set; }
}

public class OrderItemDto
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public decimal Quantity { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public ProductDetailsDto Product { get; set; }
}