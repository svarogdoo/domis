namespace domis.api.DTOs.Product;

//za prikaz osnovnih informacija o proizvodu (admin panel lista proizvoda)
public class ProductBasicInfoDto
{
    public required int Id { get; set; }
    public int? Sku { get; set; }
    public string? Name { get; set; }
}