using domis.api.DTOs.Image;

namespace domis.api.DTOs.Product;

public class ProductDetailsDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int Sku { get; set; }
    public decimal Price { get; set; }
    public decimal Stock { get; set; }
    public bool IsActive { get; set; }
    public List<ImageGetDto> Images { get; set; } = [];
    public string[] CategoryPaths { get; set; } = [];
}