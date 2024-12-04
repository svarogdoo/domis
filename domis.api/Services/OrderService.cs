using domis.api.Common;
using domis.api.DTOs.Order;
using domis.api.Models;
using domis.api.Models.Entities;
using domis.api.Repositories;

namespace domis.api.Services;

public interface IOrderService
{
    Task<IEnumerable<PaymentStatusDto>?> GetAllPaymentStatuses();
    Task<IEnumerable<OrderStatusDto>?> GetAllOrderStatuses();
    Task<IEnumerable<PaymentVendorTypeDto>?> GetAllPaymentVendorTypes();
    Task<(int InvoiceId, int? DeliveryId)> CreateOrderShipping(CreateOrderShippingRequest request);
    Task<bool> UpdateOrderShipping(int id, OrderShippingDto orderShipping);
    Task<OrderShippingDto?> GetOrderShippingById(int id);
    Task<bool> DeleteOrderShippingById(int id);
    Task<int> CreateOrderFromCart(CreateOrderRequest createOrder, UserEntity? user);
    Task<bool> UpdateOrderStatus(int orderId, int statusId);
    Task<OrderDetailsDto?> GetOrderDetailsById(int orderId);
    Task<IEnumerable<UserOrderDto>> GetOrdersByUser(string userId);
}
public class OrderService(
    IOrderRepository orderRepo, 
    IUserRepository userRepo,
    ICustomEmailSender<UserEntity> emailSender) : IOrderService
{
    public async Task<IEnumerable<PaymentStatusDto>?> GetAllPaymentStatuses() => 
        await orderRepo.GetAllPaymentStatuses();

    public async Task<IEnumerable<OrderStatusDto>?> GetAllOrderStatuses() => 
        await orderRepo.GetAllOrderStatuses();

    public async Task<IEnumerable<PaymentVendorTypeDto>?> GetAllPaymentVendorTypes() => 
        await orderRepo.GetAllPaymentVendorTypes();

    public async Task<(int InvoiceId, int? DeliveryId)> CreateOrderShipping(CreateOrderShippingRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        int? deliveryId = null;
        if (request.AddressDelivery != null)
        {
            var deliveryShippingDto = CreateDeliveryShippingDto(request);
            deliveryId = await orderRepo.CreateOrderShipping(deliveryShippingDto);
        }

        var invoiceShippingDto = CreateInvoiceShippingDto(request);

        var invoiceId = await orderRepo.CreateOrderShipping(invoiceShippingDto);

        return (invoiceId, deliveryId);
    }

    public async Task<bool> UpdateOrderShipping(int id, OrderShippingDto orderShipping) => 
        await orderRepo.UpdateOrderShipping(id, orderShipping);

    public async Task<OrderShippingDto?> GetOrderShippingById(int id) =>
        await orderRepo.GetOrderShippingById(id);

    public async Task<bool> DeleteOrderShippingById(int id) =>
        await orderRepo.DeleteOrderShippingById(id);
    
    public async Task<int> CreateOrderFromCart(CreateOrderRequest createOrder, UserEntity? user)
    {
        //var discount = await priceHelpers.GetDiscount(user);
        const int discount = 0;

        var role = user is not null
            ? await userRepo.GetUserRoleAsync(user.Id)
            : Roles.User.GetName();
        
        var order =  await orderRepo.CreateOrderFromCartAsync(createOrder, role ?? Roles.User.GetName(), discount);

        await emailSender.SendOrderConfirmationAsync(user?.Email ?? order.InvoiceAddress!.Email, order);

        return order.OrderId;
    }

    public async Task<bool> UpdateOrderStatus(int orderId, int statusId) => 
        await orderRepo.UpdateOrderStatusAsync(orderId, statusId);

    public async Task<OrderDetailsDto?> GetOrderDetailsById(int orderId) => 
        await orderRepo.GetOrderDetailsByIdAsync(orderId);

    public async Task<IEnumerable<UserOrderDto>> GetOrdersByUser(string userId)
        => await orderRepo.GetOrdersByUser(userId);
    
    private static OrderShippingDto CreateDeliveryShippingDto(CreateOrderShippingRequest request)
    {
        var deliveryShippingDto = new OrderShippingDto
        {
            FirstName = request.AddressDelivery!.FirstName,
            LastName = request.AddressDelivery.LastName,
            City = request.AddressDelivery.City,
            Address = request.AddressDelivery.AddressLine,
            Apartment = request.AddressDelivery.Apartment,
            County = request.AddressDelivery.County,
            PostalCode = request.AddressDelivery.PostalCode,
            PhoneNumber = request.AddressDelivery.PhoneNumber,
            Email = request.AddressDelivery.Email,
            AddressType = AddressType.Delivery.ToString(),
            ContactPerson = request.AddressDelivery.ContactPerson,
            ContactPhone = request.AddressDelivery.ContactPhone
        };
        return deliveryShippingDto;
    }

    private static OrderShippingDto CreateInvoiceShippingDto(CreateOrderShippingRequest request)
    {
        var invoiceShippingDto = new OrderShippingDto
        {
            FirstName = request.AddressInvoice.FirstName,
            LastName = request.AddressInvoice.LastName,
            City = request.AddressInvoice.City,
            Address = request.AddressInvoice.AddressLine,
            Apartment = request.AddressInvoice.Apartment,
            County = request.AddressInvoice.County,
            PostalCode = request.AddressInvoice.PostalCode,
            PhoneNumber = request.AddressInvoice.PhoneNumber,
            Email = request.AddressInvoice.Email,
            AddressType = AddressType.Invoice.ToString()
        };

        if (request.CompanyInfo is null) return invoiceShippingDto;
        
        invoiceShippingDto.CompanyName = request.CompanyInfo.Name;
        invoiceShippingDto.CompanyNumber = request.CompanyInfo.Number;
        invoiceShippingDto.CompanyFirstName = request.CompanyInfo.FirstName;
        invoiceShippingDto.CompanyLastName = request.CompanyInfo.LastName;

        return invoiceShippingDto;
    }
}