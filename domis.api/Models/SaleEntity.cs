namespace domis.api.Models;

public class SaleEntity
{
    public int ProductId { get; set; }
    public decimal? Price { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsActive { get; set; }
}