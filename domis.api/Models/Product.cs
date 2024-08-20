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
    //public List<string> ImageUrls { get; set; } = [];
}