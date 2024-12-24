using domis.api.DTOs.Product;
using domis.api.Models.Enums;

namespace domis.api.DTOs.Cart;

public class CartDto
{
    public int CartId { get; set; }
    public string? UserId { get; set; }
    public int StatusId { get; set; }
    public string Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<CartItemDto> Items { get; set; } = new();

    public decimal TotalCartPrice => Items.Sum(i => i.CartItemPrice * i.Quantity);
}

public class CartItemDto
{
    public int CartItemId { get; set; }
    public int ProductId { get; set; }
    public decimal Quantity { get; set; }
    public decimal CartItemPrice { get; set; }
    public DateTime CartItemCreatedAt { get; set; }
    public DateTime? CartItemModifiedAt { get; set; }
    public ProductCartDetailsDto ProductDetails { get; set; }
}

public class ProductCartDetailsDto
{
    public string? Name { get; set; }
    //public string? Description { get; set; }
    public decimal? Price { get; set; } // not null if cart item is on sale
    public string? Image { get; set; }
    public int? Sku { get; set; }
    public int? QuantityType { get; set; }
}