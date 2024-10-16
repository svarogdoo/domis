using domis.api.Common;
using domis.api.Models;
using domis.api.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using static System.Net.WebRequestMethods;

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
        
        group.MapPost("/cart-item", async Task<IResult> ([FromBody]CreateCartItemRequest request, IValidator<CreateCartItemRequest> validator, ICartService cartService, HttpContext http) =>
        {

            //var validationResults = new List<ValidationResult>();
            //var validationContext = new ValidationContext(request);
            //if (!Validator.TryValidateObject(request, validationContext, validationResults, true))
            //{
            //    return Results.BadRequest(validationResults.Select(v => v.ErrorMessage));
            //}

            var validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                return Results.BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
            }

            try
            {
                var userId = http.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var response = await cartService.CreateCartItem(request.CartId, request!.ProductId, request!.Quantity, userId);
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

        group.MapGet("/", async Task<IResult> (HttpContext http, [FromQuery] int? cartId, ICartService cartService) =>
        {
            if (cartId is not null)
            {
                // Handle guest user with cartId
                var guestCart = await cartService.GetCartWithItemsAndProductDetails((int)cartId);
                return guestCart != null 
                    ? Results.Ok(guestCart) 
                    : Results.NotFound("Cart not found for guest user.");
            }
            else
            {
                // Extract user ID from the token for authenticated users
                var userId = http.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId is null)
                {
                    return Results.Unauthorized();
                }
                // Handle authenticated user's cart
                var userCart = await cartService.GetCartByUserId(userId);
                return userCart != null
                    ? Results.Ok(userCart)
                    : Results.NotFound("Cart not found for authenticated user.");
            }
        }).WithDescription("Get cart for guest or authenticated user");
    }
}




