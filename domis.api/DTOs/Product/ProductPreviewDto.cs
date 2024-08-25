namespace domis.api.DTOs.Product;

public class ProductPreviewDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int Sku { get; set; }
    public decimal Price { get; set; }
    public decimal Stock { get; set; }
    public string? FeaturedImageUrl { get; set; }
}