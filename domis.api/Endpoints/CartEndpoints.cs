using domis.api.Common;
using domis.api.Models;
using domis.api.Models.Entities;
using domis.api.Services;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace domis.api.Endpoints;

public static class CartEndpoints
{
    public static void RegisterCartEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/cart").WithTags("Cart");
        
        group.MapGet("/cart-status", async (ICartService cartService) =>
        {
            var response = await cartService.AllOrderStatuses();

            return Results.Ok(response);
        }).WithDescription("Get all cart statuses");

        group.MapPost("/", async ([FromBody]CreateCartRequest request ,ICartService cartService) =>
        {
            var response = await cartService.CreateCart(request.UserId);

            return Results.Ok(new CreateCartResponse(response));
        }).WithDescription("Create new cart");
        
        group.MapPut("/cart-status", async ([FromBody]UpdateCartRequest request ,ICartService cartService) =>
        {
            var response = await cartService.UpdateCartStatus(request.CartId, request.StatusId);

            return Results.Ok(new UpdateCartResponse(response));
        }).WithDescription("Update cart status");
        
        group.MapDelete("/{id:int}", async ([FromRoute]int id ,ICartService cartService) =>
        {
            var response = await cartService.DeleteCart(id);

            return Results.Ok(new DeleteCartResponse(response));
        }).WithDescription("Delete cart");
        
        group.MapPost("/cart-item", async Task<IResult> ([FromBody]CreateCartItemRequest request, IValidator<CreateCartItemRequest> validator, ICartService cartService, 
            HttpContext http, UserManager<UserEntity> userManager) =>
        {
            var validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                return Results.BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
            }

            try
            {
                var user = await userManager.GetUserAsync(http.User);
                
                var response = await cartService.CreateCartItem(request.CartId, request!.ProductId, request!.Quantity, user);
                return response == null
                    ? Results.BadRequest("This product does not exist.")
                    : Results.Ok(new CreateCartItemResponse(response.Value));
            }
            catch (NotFoundException ex)
            {
                Log.Warning(ex, ex.Message);
                return Results.NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"An unexpected error occurred: {ex.Message}");
                return Results.StatusCode(500);
            }
        }).WithDescription("Create new cart item");
        
        group.MapPut("/cart-item-quantity", async ([FromBody]UpdateCartItemRequest request, ICartService cartService, HttpContext http, UserManager<UserEntity> userManager) =>
        {                
            var user = await userManager.GetUserAsync(http.User);

            var response = await cartService.UpdateCartItemQuantity(request.cartItemId, request.packageQuantity, user);

            return Results.Ok(new UpdateCartItemResponse(response));
        }).WithDescription("Update cart item quantity");
        
        group.MapDelete("/cart-item/{id:int}", async ([FromRoute]int id ,ICartService cartService) =>
        {
            var response = await cartService.DeleteCartItem(id);

            return Results.Ok(new DeleteCartItemResponse(response));
        }).WithDescription("Delete cart item");
        

        group.MapGet("/", async Task<IResult> ([FromQuery] int? cartId, ICartService cartService, 
            HttpContext http, UserManager<UserEntity> userManager) =>
        {
            var user = await userManager.GetUserAsync(http.User);

            var cart = await cartService.Cart(user, cartId);
            
            //if (user is null && cartId is null) return Results.BadRequest();

            return cart != null
                ? Results.Ok(cart)
                : Results.NotFound("Cart not found.");
        }).WithDescription("Get cart for guest or authenticated user");
    }
}




