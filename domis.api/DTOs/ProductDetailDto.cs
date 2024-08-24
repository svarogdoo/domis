namespace domis.api.DTOs;

public class ProductDetailDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int Sku { get; set; }
    public decimal Price { get; set; }
    public decimal Stock { get; set; }
    public bool IsActive { get; set; }
    public List<ImageDto> Images { get; set; } = [];
    public string[] CategoryPaths { get; set; } = [];
}