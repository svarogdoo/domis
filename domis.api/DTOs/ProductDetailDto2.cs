namespace domis.api.DTOs;

public class ProductDetailDto2
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int Sku { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public bool IsActive { get; set; }
    public List<string> ImageUrls { get; set; } = [];
}

public class ProductDetailDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int Sku { get; set; }
    public decimal Price { get; set; }
    public decimal Stock { get; set; }
    public bool IsActive { get; set; }
    public List<string> ImageUrls { get; set; } = [];
    public List<string> CategoryPaths { get; set; } = [];
}
