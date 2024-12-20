namespace domis.api.Models.Entities;

public class SaleEntity
{
    public int ProductId { get; set; }
    public decimal? SalePrice { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsActive { get; set; }
}