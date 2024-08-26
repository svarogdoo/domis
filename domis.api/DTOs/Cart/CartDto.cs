using domis.api.DTOs.Product;

namespace domis.api.DTOs.Cart;

public class CartDto
{
    public int CartId { get; set; }
    public int? UserId { get; set; }
    public int StatusId { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<CartItemDto> Items { get; set; } = new();
}

public class CartItemDto
{
    public int CartItemId { get; set; }
    public int ProductId { get; set; }
    public decimal Quantity { get; set; }
    public DateTime CartItemCreatedAt { get; set; }
    public DateTime? CartItemModifiedAt { get; set; }
    public ProductDetailsDto ProductDetails { get; set; }
}