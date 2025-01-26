namespace domis.api.DTOs.Product;

//za prikaz osnovnih informacija o proizvodu (admin panel lista proizvoda)
public class ProductBasicInfoDto
{
    public required int Id { get; set; }
    public int? Sku { get; set; }
    public string? Name { get; set; }
    public bool IsActive { get; set; }
}

public class ProductPriceDto
{
    public required int Id { get; set; }
    public decimal? Price { get; set; }
}