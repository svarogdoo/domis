using domis.api.DTOs.Order;
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
    Task<int> CreateOrderFromCart(int cartId, int paymentStatusId, int orderShippingId, int paymentVendorTypeId,
        decimal paymentAmount, string comment);

    Task<bool> UpdateOrderStatus(int orderId, int statusId);
    Task<OrderDetailsDto?> GetOrderDetailsById(int orderId);
}
public class OrderService(IOrderRepository orderRepository) : IOrderService
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


    public async Task<int> CreateOrderFromCart(int cartId, int paymentStatusId, int orderShippingId, int paymentVendorTypeId,
        decimal paymentAmount, string comment) =>
        await orderRepository.CreateOrderFromCartAsync(cartId, paymentStatusId, orderShippingId, paymentVendorTypeId, paymentAmount, comment);

    public async Task<bool> UpdateOrderStatus(int orderId, int statusId) => 
        await orderRepository.UpdateOrderStatusAsync(orderId, statusId);

    public async Task<OrderDetailsDto?> GetOrderDetailsById(int orderId) => 
        await orderRepository.GetOrderDetailsByIdAsync(orderId);

}