using domis.api.Models.Enums;

namespace domis.api.DTOs.Product;

//za prikaz proizvoda u listi proizvoda
public class ProductPreviewDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int Sku { get; set; }
    public decimal Price { get; set; }
    public decimal Stock { get; set; }
    public string? FeaturedImageUrl { get; set; }
    public string? Description { get; set; }
    public ProductQuantityType? QuantityType{ get; set; }
}