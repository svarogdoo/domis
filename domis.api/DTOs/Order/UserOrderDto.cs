namespace domis.api.DTOs.Order;

public class UserOrderDto
{
    public int Id { get; set; }
    public int? StatusId { get; set; }
    public DateTime? Date { get; set; }
    public int? PaymentTypeId { get; set; }
    public int? PaymentStatusId { get; set; }
    public decimal? PaymentAmount { get; set; }
}
