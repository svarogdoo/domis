using domis.api.Common;
using domis.api.Models;
using domis.api.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.ComponentModel.DataAnnotations;

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
        
        group.MapPost("/cart-item", async ([FromBody]CreateCartItemRequest request, IValidator<CreateCartItemRequest> validator, ICartService cartService) =>
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
                var response = await cartService.CreateCartItem(request!.CartId, request!.ProductId, request!.Quantity);
                return response == null
                    ? Results.BadRequest("Cart or Product does not exist.")
                    : Results.Ok(new CreateCartItemResponse(response.Value));
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
    }
}




