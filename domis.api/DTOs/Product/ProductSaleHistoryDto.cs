namespace domis.api.DTOs.Product;

public class ProductSaleHistoryDto
{
    public bool IsActive { get; set; }
    public decimal SalePrice { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}