using domis.api.DTOs.Order;
using domis.api.Models;
using domis.api.Services;
using Microsoft.AspNetCore.Mvc;

namespace domis.api.Endpoints;

public static class OrderEndpoints
{
    public static void RegisterOrderEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/order").WithTags("Order");
        
        group.MapGet("/payment-statuses", async (IOrderService orderService) =>
        {
            var response = await orderService.GetAllPaymentStatuses();

            return Results.Ok(response);
        }).WithDescription("Get all payment statuses");
        
        group.MapGet("/statuses", async (IOrderService orderService) =>
        {
            var response = await orderService.GetAllOrderStatuses();

            return Results.Ok(response);
        }).WithDescription("Get all order statuses");
        
        group.MapGet("/payment-vendors", async (IOrderService orderService) =>
        {
            var response = await orderService.GetAllPaymentVendorTypes();

            return Results.Ok(response);
        }).WithDescription("Get all payment vendors");
        
        group.MapPost("/shipping", async ([FromBody] OrderShippingDto request, IOrderService orderService) =>
        {
            var response = await orderService.CreateOrderShipping(request);

            return Results.Ok(new CreateOrderShippingResponse(response));
        }).WithDescription("Create new order shipping");
        
        group.MapPut("/shipping/{id}", async ([FromRoute] int id,[FromBody] OrderShippingDto request, IOrderService orderService) =>
        {
            var response = await orderService.UpdateOrderShipping(id, request);

            return Results.Ok(new UpdateOrderShippingResponse(response));
        }).WithDescription("Update order shipping");
        
        group.MapGet("/shipping/{id}", async ([FromRoute] int id, IOrderService orderService) =>
        {
            var response = await orderService.GetOrderShippingById(id);

            return Results.Ok(response);
        }).WithDescription("Get order shipping");
        
        group.MapDelete("/shipping/{id}", async ([FromRoute] int id, IOrderService orderService) =>
        {
            var response = await orderService.DeleteOrderShippingById(id);

            return Results.Ok(new DeleteOrderShippingResponse(response));
        }).WithDescription("Delete order shipping");
        
        group.MapPost("/", async ([FromBody] CreateOrderRequest request, IOrderService orderService) =>
        {
            var response = await orderService.CreateOrderFromCart(
                request.cartId, 
                request.paymentStatusId, 
                request.orderShippingId, 
                request.paymentVendorTypeId, 
                request.paymentAmount, 
                request.comment);

            return Results.Ok(new CreateOrderResponse(response));
        }).WithDescription("Create new order based on cart id");
        
        group.MapPut("/order-status", async ([FromBody] UpdateOrderRequest request, IOrderService orderService) =>
        {
            var response = await orderService.UpdateOrderStatus(request.orderId, request.statusId);

            return Results.Ok(new UpdateOrderResponse(response));
        }).WithDescription("Update order status");
        
        group.MapGet("/{id}", async ([FromRoute] int id, IOrderService orderService) =>
        {
            var response = await orderService.GetOrderDetailsById(id);

            return Results.Ok(response);
        }).WithDescription("Get order with details");
    }

}


