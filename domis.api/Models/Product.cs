using System.ComponentModel.DataAnnotations.Schema;

namespace domis.api.Models;

public class Product
{
    [Column("id")] //EF Core Data Annotation
    public int? Id { get; set; }

    [Column("product_name")]
    public string? Name { get; set; }

    [Column("product_description")]
    public string? Description { get; set; }

    [Column("sku")]
    public int? Sku { get; set; }

    [Column("price")]
    public decimal? Price { get; set; }

    [Column("stock")]
    public decimal? Stock { get; set; }

    [Column("active")]
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
    //public List<string> ImageUrls { get; set; } = [];
}