using System.ComponentModel;

namespace domis.api.Models;

public record CreateCartRequest
{
    [Description("Provide if user is logged in")]
    public int UserId { get; set; }
}

public record CreateCartResponse(int cartId);

public record UpdateCartRequest(int cartId, int statusId);
public record UpdateCartResponse(bool updated);

public record DeleteCartResponse(bool updated);

public record CreateCartItemRequest(int cartId, int productId, decimal quantity);
public record CreateCartItemResponse(int cartItemId);

public record UpdateCartItemRequest(int cartItemId, decimal quantity);
public record UpdateCartItemResponse(bool updated);

public record DeleteCartItemResponse(bool updated);