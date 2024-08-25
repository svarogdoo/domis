using System.Text.Json.Serialization;

namespace domis.api.DTOs.Product;

public class ProductEditDto
{
    [JsonPropertyName("id")]
    public required int Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("sku")]
    public int? Sku { get; set; }

    [JsonPropertyName("price")]
    public decimal? Price { get; set; }

    [JsonPropertyName("stock")]
    public decimal? Stock { get; set; }

    //new fields
    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("width")]
    public decimal? Width { get; set; }

    [JsonPropertyName("height")]
    public decimal? Height { get; set; }

    [JsonPropertyName("weight")]
    public decimal? Weight { get; set; }

    [JsonPropertyName("depth")]
    public decimal? Depth { get; set; }

    [JsonPropertyName("length")]
    public decimal? Length { get; set; }

    [JsonPropertyName("thickness")]
    public decimal? Thickness { get; set; }

    [JsonPropertyName("isItemType")]
    public bool? IsItem { get; set; }

    [JsonPropertyName("isSurfaceType")]
    public bool? IsSurfaceType { get; set; }
}