namespace domis.api.DTOs.Product;

public class ProductUpdateResponseDto
{
    public required int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public required int Sku { get; set; }
    public decimal? Price { get; set; }
    public decimal? Stock { get; set; }
    public bool? IsActive { get; set; }
    public string? Title { get; set; }
    public decimal? Width { get; set; }
    public decimal? Height { get; set; }
    public decimal? Depth { get; set; }
    public decimal? Length { get; set; }
    public decimal? Thickness { get; set; }
    public decimal? Weight { get; set; }
    public bool? IsItemType { get; set; }
    public bool? IsSurfaceType { get; set; }
    public string? FeaturedImageUrl { get; set; }
}
