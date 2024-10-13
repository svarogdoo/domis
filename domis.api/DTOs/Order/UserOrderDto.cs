namespace domis.api.DTOs.Order;

public class UserOrderDto
{
    public int Id { get; set; }
    public int StatusId { get; set; }
    public decimal PaymentAmount { get; set; }
    public string? Comment { get; set; }
}
