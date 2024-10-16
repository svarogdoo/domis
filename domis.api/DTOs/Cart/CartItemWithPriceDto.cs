namespace domis.api.DTOs.Cart;

public class CartItemWithPriceDto
{
    public int CartItemId { get; set; }
    public int CartId { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public decimal ProductPrice { get; set; }
}