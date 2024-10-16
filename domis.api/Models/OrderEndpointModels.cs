namespace domis.api.Models;

public record CreateOrderShippingResponse(int orderShippingId);
public record UpdateOrderShippingResponse(bool updated);
public record DeleteOrderShippingResponse(bool deleted);


public record CreateOrderRequest(int cartId, int paymentStatusId, int orderShippingId, int paymentVendorTypeId, string comment);
public record CreateOrderResponse(int orderId);

public record UpdateOrderRequest(int orderId, int statusId);
public record UpdateOrderResponse(bool updated);