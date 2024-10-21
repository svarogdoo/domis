using System.Data;
using Dapper;
using domis.api.Common;
using domis.api.DTOs.Cart;
using domis.api.DTOs.Order;
using domis.api.Models;
using domis.api.Repositories.Queries;
using Serilog;

namespace domis.api.Repositories;

public interface IOrderRepository
{
    Task<IEnumerable<PaymentStatusDto>?> GetAllPaymentStatuses();
    Task<IEnumerable<OrderStatusDto>?> GetAllOrderStatuses();
    Task<IEnumerable<PaymentVendorTypeDto>?> GetAllPaymentVendorTypes();
    Task<int> CreateOrderShipping(OrderShippingDto orderShipping);
    Task<bool> UpdateOrderShipping(int id, OrderShippingDto orderShipping);
    Task<OrderShippingDto?> GetOrderShippingById(int id);
    Task<bool> DeleteOrderShippingById(int id);
    Task<int> CreateOrderFromCartAsync(CreateOrderRequest createOrder, decimal discount);

    Task<bool> UpdateOrderStatusAsync(int orderId, int statusId);
    Task<OrderDetailsDto?> GetOrderDetailsByIdAsync(int orderId);
    Task<IEnumerable<UserOrderDto>> GetOrdersByUserIdAsync(string userId);
}
public class OrderRepository(IDbConnection connection) : IOrderRepository
{
    public async Task<IEnumerable<PaymentStatusDto>?> GetAllPaymentStatuses()
    {
        try
        {
            var paymentStatuses = 
                (await connection.QueryAsync<PaymentStatusDto>(OrderQueries.GetPaymentStatuses))
                .ToList();

            return paymentStatuses.Any() ? paymentStatuses : null;
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"An error occurred while fetching payment statuses: {ex.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<OrderStatusDto>?> GetAllOrderStatuses()
    {
        try
        {
            var orderStatuses =
                (await connection.QueryAsync<OrderStatusDto>(OrderQueries.GetOrderStatuses))
                .ToList();

            return orderStatuses.Any() ? orderStatuses : null;
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"An error occurred while fetching order statuses: {ex.Message}");
            throw;
        }
    }
   
    public async Task<IEnumerable<PaymentVendorTypeDto>?> GetAllPaymentVendorTypes()
    {
        try
        {
            var paymentVendorTypes = 
                (await connection.QueryAsync<PaymentVendorTypeDto>(OrderQueries.GetPaymentVendors))
                .ToList();

            return paymentVendorTypes.Any() ? paymentVendorTypes : null;
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"An error occurred while fetching payment vendor types: {ex.Message}");
            throw;
        }
    }

    public async Task<int> CreateOrderShipping(OrderShippingDto orderShipping)
    {
        try
        {
            var orderShippingId =
                await connection.ExecuteScalarAsync<int>(OrderQueries.CreateOrderShipping, orderShipping);

            return orderShippingId;
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"An error occurred while creating order shipping: {ex.Message}");
            throw;
        }
    }
    
    public async Task<bool> UpdateOrderShipping(int id, OrderShippingDto orderShipping)
    {
        try
        {
            var rowsAffected = await connection.ExecuteAsync(OrderQueries.UpdateOrderShipping, new
            {
                Id = id,
                orderShipping.FirstName,
                orderShipping.LastName,
                orderShipping.CompanyName,
                orderShipping.CountryId,
                orderShipping.City,
                orderShipping.Address,
                orderShipping.Apartment,
                orderShipping.County,
                orderShipping.PostalCode,
                orderShipping.PhoneNumber,
                orderShipping.Email
            });

            return rowsAffected > 0; 
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"An error occurred while updating order shipping with ID {id}: {ex.Message}");
            throw;
        }
    }
    
    public async Task<OrderShippingDto?> GetOrderShippingById(int id)
    {
        try
        {
            var result = await connection.QuerySingleOrDefaultAsync<OrderShippingWithCountryDto>(OrderQueries.GetOrderShipping, new { Id = id });

            return result;
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"An error occurred while fetching order shipping with ID {id}: {ex.Message}");
            throw;
        }
    }
    
    public async Task<bool> DeleteOrderShippingById(int id)
    {
        try
        {
            var rowsAffected = await connection.ExecuteAsync(OrderQueries.DeleteOrderShipping
                , new { Id = id });

            return rowsAffected > 0; 
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"An error occurred while deleting order shipping with ID {id}: {ex.Message}");
            throw;
        }
    }
    
    public async Task<int> CreateOrderFromCartAsync(CreateOrderRequest createOrder, decimal discount = 0)
    {
        connection.Open();
        using var transaction = connection.BeginTransaction();
        try
        {
            //get cart items with product price
            var cartItems = await connection.QueryAsync<CartItemWithPriceDto>(CartQueries.GetCartItemsWithProductPriceByCartId, new { CartId = createOrder.cartId }, transaction);
            //calculate order total amount
            //TODO: include role discount
            var totalAmount = cartItems.Sum(i => PricingHelper.CalculateDiscount(i.ProductPrice * i.Quantity, discount));

            //create order from cart
            var orderId = await connection.QuerySingleAsync<int>(OrderQueries.CreateOrder, new
            {
                CartId = createOrder.cartId,
                OrderShippingId = createOrder.orderShippingId,
                PaymentStatusId = createOrder.paymentStatusId,
                PaymentVendorTypeId = createOrder.paymentVendorTypeId,
                PaymentAmount = totalAmount,
                Comment = createOrder.comment,
                CreatedAt = DateTime.UtcNow
            }, transaction);

            //create order items from cart items
            await connection.ExecuteAsync(OrderQueries.CreateOrderItems, new
            {
                OrderId = orderId,
                CartId = createOrder.cartId,
                CreatedAt = DateTime.UtcNow
            }, transaction);

            //get all order items

            var orderItems = await connection.QueryAsync<OrderItemWithPriceDto>(OrderQueries.GetOrderItemsWithPrices,new 
            { OrderId = orderId 
            }, transaction);

             //update order items with calculated prices
        
            foreach (var orderItem in orderItems)
            {
                await connection.ExecuteAsync(OrderQueries.UpdateOrderItemPrice, new
                {
                    ProductPrice = PricingHelper.CalculateDiscount(orderItem.ProductPrice, discount), // Updated price here
                    OrderItemId = orderItem.OrderItemId
                }, transaction);
            }


        //flag cart status to completed/order
        //TODO: maybe we don't need this if we're already deleting the cart
        await connection.ExecuteAsync(CartQueries.UpdateCartStatus, new
            {
                CartId = createOrder.cartId,
                StatusId = 3 //converted to order
            }, transaction);

            await connection.ExecuteAsync(CartQueries.DeleteCartItemsQuery, new { CartId = createOrder.cartId }, transaction);
            await connection.ExecuteAsync(CartQueries.DeleteCartQuery, new { CartId = createOrder.cartId }, transaction);

            transaction.Commit();

            return orderId;
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            Log.Error(ex, $"An error occurred while creating an order from cart ID {createOrder.cartId}: {ex.Message}");
            throw;
        }
        finally
        {
            connection.Close();
        }
    }
    
    public async Task<bool> UpdateOrderStatusAsync(int orderId, int statusId)
    {
        try
        {
            var rowsAffected = await connection.ExecuteAsync(OrderQueries.UpdateOrderStatus, new 
            {
                OrderId = orderId,
                StatusId = statusId
            });

            return rowsAffected > 0;
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"An error occurred while updating the status of order with ID {orderId}: {ex.Message}");
            throw;
        }
    }

    public async Task<OrderDetailsDto?> GetOrderDetailsByIdAsync(int orderId)
    {
        try
        {
            OrderDetailsDto? orderDetails = null;

            var result = await connection
                .QueryAsync<OrderDetailsDto, OrderStatusdetialsDto, OrderShippingDetailsDto, PaymentDetailsDto, OrderItemDto,
                    ProductOrderDetailsDto, string, OrderDetailsDto>(
                    OrderQueries.GetOrderDetails,
                    (order, orderStatus, orderShipping, paymentDetails, orderItem, product, url) =>
                    {

                        if (orderDetails == null)
                        {
                            orderDetails = order;
                            orderDetails.OrderStatus = orderStatus;
                            orderDetails.OrderShipping = orderShipping;
                            orderDetails.PaymentDetails = paymentDetails;
                        }

                        if (orderItem != null &&
                            orderDetails.OrderItems.Find(oi => oi.OrderItemId == orderItem.OrderItemId) == null)
                        {
                            orderItem.ProductDetails = product;
                            orderItem.ProductDetails.Url = url;
                            orderDetails.OrderItems.Add(orderItem);
                        }

                        return orderDetails;
                    },
                    new { OrderId = orderId },
                    splitOn: "OrderStatusId,OrderShippingId,PaymentStatusId,OrderItemId,ProductName, Url"
                );

            return result.FirstOrDefault();
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"An error occurred while fetching order details with ID {orderId}: {ex.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<UserOrderDto>> GetOrdersByUserIdAsync(string userId)
    {
        try
        {
            var userOrders = await connection.QueryAsync<UserOrderDto>(OrderQueries.GetOrdersByUserId, new { UserId = userId });

            return userOrders;
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"An error occurred while retrieving orders for user with ID {userId}: {ex.Message}");
            throw;
        }
    }
}