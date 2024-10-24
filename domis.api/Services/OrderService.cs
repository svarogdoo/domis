using domis.api.Common;
using domis.api.DTOs.Order;
using domis.api.Models;
using domis.api.Repositories;

namespace domis.api.Services;

public interface IOrderService
{
    Task<IEnumerable<PaymentStatusDto>?> GetAllPaymentStatuses();
    Task<IEnumerable<OrderStatusDto>?> GetAllOrderStatuses();
    Task<IEnumerable<PaymentVendorTypeDto>?> GetAllPaymentVendorTypes();
    Task<int> CreateOrderShipping(OrderShippingDto orderShipping);
    Task<bool> UpdateOrderShipping(int id, OrderShippingDto orderShipping);
    Task<OrderShippingDto?> GetOrderShippingById(int id);
    Task<bool> DeleteOrderShippingById(int id);
    Task<int> CreateOrderFromCart(CreateOrderRequest createOrder, UserEntity? user);

    Task<bool> UpdateOrderStatus(int orderId, int statusId);
    Task<OrderDetailsDto?> GetOrderDetailsById(int orderId);

    Task<IEnumerable<UserOrderDto>> GetOrdersByUser(string userId);
}
public class OrderService(IOrderRepository orderRepository, IPriceHelpers priceHelpers, ICustomEmailSender<UserEntity> emailSender) : IOrderService
{
    public async Task<IEnumerable<PaymentStatusDto>?> GetAllPaymentStatuses() => 
        await orderRepository.GetAllPaymentStatuses();

    public async Task<IEnumerable<OrderStatusDto>?> GetAllOrderStatuses() => 
        await orderRepository.GetAllOrderStatuses();

    public async Task<IEnumerable<PaymentVendorTypeDto>?> GetAllPaymentVendorTypes() => 
        await orderRepository.GetAllPaymentVendorTypes();


    public async Task<int> CreateOrderShipping(OrderShippingDto orderShipping) =>
        await orderRepository.CreateOrderShipping(orderShipping);


    public async Task<bool> UpdateOrderShipping(int id, OrderShippingDto orderShipping) => 
        await orderRepository.UpdateOrderShipping(id, orderShipping);


    public async Task<OrderShippingDto?> GetOrderShippingById(int id) =>
        await orderRepository.GetOrderShippingById(id);


    public async Task<bool> DeleteOrderShippingById(int id) =>
        await orderRepository.DeleteOrderShippingById(id);


    public async Task<int> CreateOrderFromCart(CreateOrderRequest createOrder, UserEntity? user)
    {
        var discount = await priceHelpers.GetDiscount(user);

        var order =  await orderRepository.CreateOrderFromCartAsync(createOrder, discount);

        await emailSender.SendOrderConfirmationAsync(user?.Email ?? order.Shipping!.Email, order);

        return order.OrderId;
    }

    public async Task<bool> UpdateOrderStatus(int orderId, int statusId) => 
        await orderRepository.UpdateOrderStatusAsync(orderId, statusId);

    public async Task<OrderDetailsDto?> GetOrderDetailsById(int orderId) => 
        await orderRepository.GetOrderDetailsByIdAsync(orderId);

    public async Task<IEnumerable<UserOrderDto>> GetOrdersByUser(string userId)
        => await orderRepository.GetOrdersByUser(userId);
}