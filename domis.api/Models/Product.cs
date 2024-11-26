using domis.api.Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace domis.api.Models;

public class Product
{
    [Column("id")]
    public int? Id { get; init; }

    [Column("product_name")]
    public string? Name { get; init; }

    [Column("product_description")]
    public string? Description { get; init; }

    [Column("sku")]
    public int? Sku { get; set; }

    [Column("price")]
    public decimal? Price { get; set; }

    [Column("stock")]
    public decimal? Stock { get; init; }

    [Column("active")]
    public bool? IsActive { get; init; }

    public string? Title { get; init; }
    public decimal? Width { get; init; }
    public decimal? Height { get; init; }
    public decimal? Depth { get; init; }
    public decimal? Length { get; init; }
    public decimal? Thickness { get; init; }
    public decimal? Weight { get; init; }
    public bool? IsItemType { get; init; }
    public bool? IsSurfaceType { get; init; }
    public string? FeaturedImageUrl { get; init; }
    //public List<string> ImageUrls { get; set; } = [];
    //public ProductQuantityType? QuantityType { get; set; }
    public int QuantityType { get; init; } // Use this property to store the quantity type ID

}