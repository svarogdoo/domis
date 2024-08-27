using System.ComponentModel;
using domis.api.Services;
using Microsoft.AspNetCore.Mvc;

namespace domis.api.Endpoints;

public static class CartEndpoints
{
    public static void RegisterCartEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/cart").WithTags("Cart");
        
        group.MapGet("/cart-status", async (ICartService cartService) =>
        {
            var response = await cartService.GetAllOrderStatuses();

            return Results.Ok(response);
        }).WithDescription("Get all cart statuses");

        group.MapPost("/", async ([FromBody]CreateCartRequest request ,ICartService cartService) =>
        {
            var response = await cartService.CreateCart(request.UserId);

            return Results.Ok(new CreateCartResponse(response));
        }).WithDescription("Create new cart");
        
        group.MapPut("/cart-status", async ([FromBody]UpdateCartRequest request ,ICartService cartService) =>
        {
            var response = await cartService.UpdateCartStatus(request.cartId, request.statusId);

            return Results.Ok(new UpdateCartResponse(response));
        }).WithDescription("Update cart status");
        
        group.MapDelete("/{id}", async ([FromRoute]int id ,ICartService cartService) =>
        {
            var response = await cartService.DeleteCart(id);

            return Results.Ok(new DeleteCartResponse(response));
        }).WithDescription("Delete cart");
        
        group.MapPost("/cart-item", async ([FromBody]CreateCartItemRequest request ,ICartService cartService) =>
        {
            var response = await cartService.CreateCartItem(request.cartId, request.productId, request.quantity);

            return Results.Ok(new CreateCartItemResponse(response));
        }).WithDescription("Create new cart item");
        
        group.MapPut("/cart-item-quantity", async ([FromBody]UpdateCartItemRequest request ,ICartService cartService) =>
        {
            var response = await cartService.UpdateCartItemQuantity(request.cartItemId, request.quantity);

            return Results.Ok(new UpdateCartItemResponse(response));
        }).WithDescription("Update cart item quantity");
        
        group.MapDelete("/cart-item/{id}", async ([FromRoute]int id ,ICartService cartService) =>
        {
            var response = await cartService.DeleteCartItem(id);

            return Results.Ok(new DeleteCartItemResponse(response));
        }).WithDescription("Delete cart item");
        
        group.MapGet("/{id}", async ([FromRoute]int id, ICartService cartService) =>
        {
            var response = await cartService.GetCartWithItemsAndProductDetails(id);

            return Results.Ok(response);
        }).WithDescription("Get cart with details");
    }
}

public record CreateCartRequest
{
    [Description("Provide if user is logged in")]
    public int UserId { get; set; }
}

public record CreateCartResponse(int cartId);

public record UpdateCartRequest(int cartId, int statusId);
public record UpdateCartResponse(bool updated);

public record DeleteCartRequest(int cartId);
public record DeleteCartResponse(bool updated);

public record CreateCartItemRequest(int cartId, int productId, decimal quantity);
public record CreateCartItemResponse(int cartItemId);
public record UpdateCartItemRequest(int cartItemId, decimal quantity);
public record UpdateCartItemResponse(bool updated);

public record DeleteCartItemRequest(int cartItemId);
public record DeleteCartItemResponse(bool updated);


