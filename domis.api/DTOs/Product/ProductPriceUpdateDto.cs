namespace domis.api.DTOs.Product;

public class ProductPriceUpdateDto
{
    public decimal? RegularPrice { get; set; }
    public List<ProductVpPriceUpdateDto>? VpPrices { get; set; }
}

public class ProductVpPriceUpdateDto
{
    public required string UserType { get; set; } //VP1-VP4
    public decimal? PakPrice { get; set; }
    public decimal? PalPrice { get; set; }
}