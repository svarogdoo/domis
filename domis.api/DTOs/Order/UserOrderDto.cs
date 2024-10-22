namespace domis.api.DTOs.Order;

public class UserOrderDto
{
    public int Id { get; set; }
    public int? StatusId { get; set; }
    public DateTime? Date { get; set; }
    public string? Address { get; set; }
    public int? PaymentTypeId { get; set; }
    public int? PaymentStatusId { get; set; }
    public decimal? PaymentAmount { get; set; }
    public List<UserOrderItem> OrderItems { get; set; } = []; 
}

public class UserOrderItem
{
    public int Id { get; set; }
    public int Quantity { get; set; }
    public decimal ItemPrice { get; set; }
    public ProductDetails? ProductDetails { get; set; }
}