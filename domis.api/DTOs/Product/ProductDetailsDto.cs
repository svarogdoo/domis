using domis.api.DTOs.Image;
using domis.api.Models.Enums;
using System.Text.Json.Serialization;
using domis.api.DTOs.Category;

namespace domis.api.DTOs.Product;

public class ProductDetailsDto
{
    public required int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public required int Sku { get; set; }
    public Price? Price { get; set; }
    public VpPrice? VpPrice { get; set; }
    public decimal? Stock { get; set; }
    public bool? IsActive { get; set; }
    public Size? Size { get; set; }
    public string? FeaturedImageUrl { get; set; }
    public List<ImageGetDto> Images { get; set; } = [];
    public IEnumerable<IEnumerable<CategoryPath>> CategoryPaths { get; set; } = [];
    public SaleInfo? SaleInfo { get; set; }
    public Attributes? Attributes { get; set; }
}

public class Price
{
    [JsonPropertyName("perUnit")]
    public decimal? Unit { get; set; }
    
    [JsonPropertyName("perBox")]
    public decimal? Pak { get; set; }
    
    [JsonPropertyName("perPallet")]
    public decimal? Pal { get; set; }
}

public class VpPrice
{
    [JsonPropertyName("perUnit")]
    public decimal? PakUnitPrice { get; set; }
    
    [JsonPropertyName("perPalletUnit")]
    public decimal? PalUnitPrice { get; set; }
    
    [JsonPropertyName("perBox")]
    public decimal? PakPrice { get; set; }
    
    [JsonPropertyName("perPallet")]
    public decimal? PalPrice { get; set; }
}

public class Size
{
    [JsonPropertyName("box")]
    public string? Pak { get; set; }
    
    [JsonPropertyName("pallet")]
    public string? Pal { get; set; }
}

public class SaleInfo
{
    public bool IsActive { get; set; }
    public decimal? SalePrice { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}

public class Attributes
{    
    public int? QuantityType { get; set; }
    public string? Title { get; set; }
    public decimal? Width { get; set; }
    public decimal? Height { get; set; }
    public decimal? Depth { get; set; }
    public decimal? Length { get; set; }
    public decimal? Thickness { get; set; }
    public decimal? Weight { get; set; }
}