namespace domis.api.Models;

public class Sale
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public decimal? SalePrice { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsActive { get; set; }
}