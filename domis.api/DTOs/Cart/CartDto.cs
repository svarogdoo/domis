using domis.api.DTOs.Product;

namespace domis.api.DTOs.Cart;

public class CartDto
{
    public int CartId { get; set; }
    public int? UserId { get; set; }
    public int StatusId { get; set; }
    public string Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<CartItemDto> Items { get; set; } = new();

    public decimal TotalCartPrice => Items.Sum(i => i.CartItemPrice);
}

public class CartItemDto
{
    public int CartItemId { get; set; }
    public int ProductId { get; set; }
    public decimal Quantity { get; set; }
    public DateTime CartItemCreatedAt { get; set; }
    public DateTime? CartItemModifiedAt { get; set; }
    public ProductCartDetailsDto ProductDetails { get; set; }
    public decimal CartItemPrice => ProductDetails.Price * Quantity;
}

public class ProductCartDetailsDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string? Image { get; set; }
}