using System.Data;
using Dapper;
using domis.api.Common;
using domis.api.DTOs.Cart;
using domis.api.DTOs.Order;
using domis.api.Models;
using domis.api.Repositories.Queries;
using MailKit.Search;
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
    Task<IEnumerable<OrderShippingDto>?> GetOrderShippingListById(int id);
    Task<bool> DeleteOrderShippingById(int id);
    Task<OrderConfirmationDto> CreateOrderFromCartAsync(CreateOrderRequest createOrder, string role, decimal discount);
    Task<bool> UpdateOrderStatusAsync(int orderId, int statusId);
    Task<OrderDetailsDto?> GetOrderDetailsByIdAsync(int orderId);
    Task<IEnumerable<UserOrderDto>> GetOrdersByUser(string userId);
    Task<IEnumerable<OrderDetailsDto>> GetOrders();
}
public class OrderRepository(IDbConnection connection, PriceCalculationHelper helper) : IOrderRepository
{
    public async Task<IEnumerable<PaymentStatusDto>?> GetAllPaymentStatuses()
    {
        try
        {
            var paymentStatuses = 
                (await connection.QueryAsync<PaymentStatusDto>(OrderQueries.GetPaymentStatuses))
                .ToList();

            return paymentStatuses.Count != 0 ? paymentStatuses : null;
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

            return orderStatuses.Count != 0 ? orderStatuses : null;
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

            return paymentVendorTypes.Count != 0 ? paymentVendorTypes : null;
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
            return await connection.ExecuteScalarAsync<int>(OrderQueries.CreateOrderShipping, orderShipping);
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
                orderShipping.Email,
                orderShipping.CompanyNumber,
                orderShipping.CompanyFirstName,
                orderShipping.CompanyLastName,
                orderShipping.ContactPhone,
                orderShipping.ContactPerson,
                orderShipping.AddressType
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
            return await connection.QuerySingleOrDefaultAsync<OrderShippingWithCountryDto>(OrderQueries.GetOrderShipping, new { Id = id });
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"An error occurred while fetching order shipping with ID {id}: {ex.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<OrderShippingDto>?> GetOrderShippingListById(int id)
    {
        try
        {
            return await connection.QueryAsync<OrderShippingWithCountryDto>(OrderQueries.GetOrderShipping, new { Id = id });
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"An error occurred while fetching order shipping with ID {id}: {ex.Message}");
            throw;
        }    }

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
    
    public async Task<OrderConfirmationDto> CreateOrderFromCartAsync(CreateOrderRequest createOrder, string role, decimal discount = 0)
    {
        connection.Open();
        using var transaction = connection.BeginTransaction();
        
        try
        {
            var cartItems = (await connection.QueryAsync<CartItemWithPriceDto>(
                    CartQueries.GetCartItemsWithProductPriceByCartId, new 
            {
                createOrder.CartId 
            }, transaction))
            .ToList();
            
            ValidateCartItems(role, cartItems, helper);

            var totalAmount = CalculateTotalAmount(cartItems, discount);
            
            var orderId = await CreateOrderAsync(createOrder, totalAmount, transaction);

            await CreateOrderItemsAsync(orderId, createOrder.CartId, transaction);

            await FinalizeCartAsync(createOrder.CartId, transaction);
            
            transaction.Commit();
            
            var orderItems = await connection.QueryAsync<OrderItemWithPriceDto>(OrderQueries.GetOrderItemsWithPrices, new 
            { 
                OrderId = orderId 
            }, transaction);
            
            return new OrderConfirmationDto
            {
                OrderId = orderId,
                OrderItems = orderItems.ToList(),
                TotalPrice = totalAmount,
                InvoiceAddress = await GetOrderShippingById(createOrder.InvoiceOrderShippingId),
                DeliveryAddress = createOrder.DeliveryOrderShippingId.HasValue 
                    ? await GetOrderShippingById(createOrder.DeliveryOrderShippingId.Value) 
                    : null
            };
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            Log.Error(ex, $"An error occurred while creating an order from cart ID {createOrder.CartId}: {ex.Message}");
            throw;
        }
        finally
        {
            connection.Close();
        }
    }
    
    private static async void ValidateCartItems(string userRole, List<CartItemWithPriceDto>? cartItems, PriceCalculationHelper helper)
    {
        if (cartItems == null || cartItems.Count == 0)
            throw new InvalidOperationException("Cart is empty. Cannot create an order.");
    
        foreach (var cartItem in cartItems)
        {
            var sizing = await helper.GetProductSizing(cartItem.ProductId);
            var palSize = PriceCalculationHelper.PalSizeAsNumber(sizing);
            var expectedPrice = await helper.GetPriceBasedOnRoleAndQuantity(
                cartItem.ProductId, userRole, cartItem.Quantity, palSize);

            // If the price hasn't changed, do nothing
            if (cartItem.ProductPrice == expectedPrice) continue;
            
            // Log or notify about the price change, if necessary
            Log.Information($"Price mismatch for product ID {cartItem.ProductId}: Expected price {expectedPrice}, but found {cartItem.ProductPrice}");
            if (expectedPrice != null) cartItem.ProductPrice = (decimal)expectedPrice;
        }
    }

    
    private static decimal CalculateTotalAmount(IEnumerable<CartItemWithPriceDto> cartItems, decimal discount)
    {
        var total = cartItems.Sum(i => i.ProductPrice * i.Quantity);
        var discountedTotal = total - total * discount; //discount not needed
        
        return total;
    }
    
    private async Task<int> CreateOrderAsync(CreateOrderRequest createOrder, decimal totalAmount, IDbTransaction transaction)
    {
        return await connection.QuerySingleAsync<int>(OrderQueries.CreateOrder, new
        {
            createOrder.CartId,
            createOrder.InvoiceOrderShippingId,
            createOrder.DeliveryOrderShippingId,
            createOrder.PaymentStatusId,
            createOrder.PaymentVendorTypeId,
            PaymentAmount = totalAmount,
            createOrder.Comment,
            CreatedAt = DateTimeHelper.BelgradeNow
        }, transaction);
    }
    
    private async Task CreateOrderItemsAsync(int orderId, int cartId, IDbTransaction transaction)
    {
        await connection.ExecuteAsync(OrderQueries.CreateOrderItems, new
        {
            OrderId = orderId,
            CartId = cartId,
            CreatedAt = DateTimeHelper.BelgradeNow
        }, transaction);
    }
    
    private async Task FinalizeCartAsync(int cartId, IDbTransaction transaction)
    {
        await connection.ExecuteAsync(CartQueries.UpdateCartStatus, new
        {
            CartId = cartId,
            StatusId = 3 //convert to order
        }, transaction);

        await connection.ExecuteAsync(CartQueries.DeleteCartItemsQuery, new { CartId = cartId }, transaction);
        await connection.ExecuteAsync(CartQueries.DeleteCartQuery, new { CartId = cartId }, transaction);
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
                .QueryAsync<OrderDetailsDto, OrderStatusDetailsDto, OrderShippingDetailsDto, PaymentDetailsDto, OrderItemDto,
                    ProductDetails, string, OrderDetailsDto>(
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

    public async Task<IEnumerable<UserOrderDto>> GetOrdersByUser(string userId)
    {
        try
        {
            var orderDict = new Dictionary<int, UserOrderDto>();

            var userOrders = await connection.QueryAsync<UserOrderDto>(OrderQueries.GetOrdersByUserId, new { UserId = userId });

            foreach (var order in userOrders)
            {
                if (!orderDict.TryGetValue(order.Id, out var userOrder))
                {
                    userOrder = new UserOrderDto
                    {
                        Id = order.Id,
                        StatusId = order.StatusId,
                        Date = order.Date,
                        Address = order.Address,
                        PaymentTypeId = order.PaymentTypeId,
                        PaymentStatusId = order.PaymentStatusId,
                        PaymentAmount = order.PaymentAmount,
                    };

                    orderDict.Add(order.Id, userOrder);
                }

                // Fetch the order items for the current order
                var orderItems = await connection.QueryAsync<UserOrderItem, OrderItemProduct, UserOrderItem>(
                    OrderQueries.GetOrderItemsByOrderId,
                    (orderItem, productDetails) =>
                    {
                        // Assign product details
                        orderItem.ProductDetails = productDetails;
                        return orderItem; // Return the order item with product details
                    },
                    new { OrderId = order.Id },
                    splitOn: "Name"
                );

                // Add the fetched order items to the user order
                userOrder.OrderItems.AddRange(orderItems);
            }

            return orderDict.Values;
            //return userOrders;
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"An error occurred while retrieving orders for user with ID {userId}: {ex.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<OrderDetailsDto>> GetOrders()
    {
        try
        {
            var orderDictionary = new Dictionary<int, OrderDetailsDto>();

            var result = await connection
                .QueryAsync<OrderDetailsDto, OrderStatusDetailsDto, OrderShippingDetailsDto, PaymentDetailsDto, OrderItemDto,
                    ProductDetails, string, OrderDetailsDto>(
                    OrderQueries.GetAllOrders,
                    (order, orderStatus, orderShipping, paymentDetails, orderItem, product, url) =>
                    {
                        if (!orderDictionary.TryGetValue(order.OrderId, out var orderDetails))
                        {
                            orderDetails = order;
                            orderDetails.OrderStatus = orderStatus;
                            orderDetails.OrderShipping = orderShipping;
                            orderDetails.PaymentDetails = paymentDetails;
                            orderDetails.OrderItems = new List<OrderItemDto>();
                            orderDictionary.Add(order.OrderId, orderDetails);
                        }

                        if (orderDetails.OrderItems.Any(oi => oi.OrderItemId == orderItem.OrderItemId))
                            return orderDetails;
                        
                        orderItem.ProductDetails = product;
                        orderItem.ProductDetails.Url = url;
                        orderDetails.OrderItems.Add(orderItem);

                        return orderDetails;
                    },
                    splitOn: "OrderStatusId,OrderShippingId,PaymentStatusId,OrderItemId,ProductName,Url"
                );

            return orderDictionary.Values.OrderByDescending(o => o.OrderId).ToList();
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"An error occurred while fetching orders: {ex.Message}");
            throw;
        }    
    }
}