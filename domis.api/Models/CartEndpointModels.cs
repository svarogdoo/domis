using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace domis.api.Models;

public record CreateCartRequest
{
    [Description("Provide if user is logged in")]
    public string? UserId { get; set; }
}

public record CreateCartResponse(int CartId);

public record UpdateCartRequest(int CartId, int StatusId);
public record UpdateCartResponse(bool Updated);

public record DeleteCartResponse(bool Deleted);

//public record CreateCartItemRequest(
//    [Required] [Range(1, int.MaxValue, ErrorMessage = "CartId must be greater than zero.")] int cartId, 
//    [Required] [Range(1, int.MaxValue, ErrorMessage = "ProductId must be greater than zero.")] int productId, 
//    [Required] [Range(0.01, double.MaxValue, ErrorMessage = "Quantity must be greater than zero.")]decimal quantity
//);

public record CreateCartItemRequest(int? CartId, int ProductId, decimal Quantity);


//public record CreateCartItemRequest(
//    [property: Required]
//    [property: Range(1, int.MaxValue, ErrorMessage = "CartId must be greater than zero.")]
//    int CartId,

//    [property: Required]
//    [property: Range(1, int.MaxValue, ErrorMessage = "ProductId must be greater than zero.")]
//    int ProductId,

//    [property: Required]
//    [property: Range(0.01, double.MaxValue, ErrorMessage = "Quantity must be greater than zero.")]
//    decimal Quantity);

//public class CreateCartItemRequest
//{
//    [Required]
//    [Range(1, int.MaxValue, ErrorMessage = "CartId must be greater than zero.")]
//    public int CartId { get; set; }
//    [Required]
//    [Range(1, int.MaxValue, ErrorMessage = "ProductId must be greater than zero.")]
//    public int ProductId { get; set; }
//    [Required]
//    [Range(0.01, double.MaxValue, ErrorMessage = "Quantity must be greater than zero.")]
//    public decimal Quantity { get; set; }
//}

public record CreateCartItemResponse(int? cartId);

public record UpdateCartItemRequest(int cartItemId, decimal packageQuantity);
public record UpdateCartItemResponse(bool updated);

public record DeleteCartItemResponse(bool deleted);