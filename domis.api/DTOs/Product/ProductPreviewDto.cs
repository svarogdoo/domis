using domis.api.Models.Enums;

namespace domis.api.DTOs.Product;

//za prikaz proizvoda u listi proizvoda
public class ProductPreviewDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int Sku { get; set; }
    public decimal Price { get; set; }
    public decimal? VpPrice { get; set; }
    public decimal Stock { get; set; }
    public string? FeaturedImageUrl { get; set; }
    public string? Description { get; set; }
    public ProductQuantityType? QuantityType{ get; set; }
    public SaleInfo? SaleInfo { get; set; }
    // public bool IsOnSale { get; set; }
    // public decimal? SalePrice { get; set; }
    // public DateTime? SaleStartDate { get; set; }
    // public DateTime? SaleEndDate { get; set; }
}