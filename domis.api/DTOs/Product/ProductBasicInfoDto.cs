namespace domis.api.DTOs.Product;

public class ProductBasicInfoDto
{
    public required int Id { get; set; }
    public int? Sku { get; set; }
    public string? Name { get; set; }
}