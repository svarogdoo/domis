namespace domis.api.Models;

public record CreateOrderShippingResponse(int OrderShippingId);
public record UpdateOrderShippingResponse(bool Updated);
public record DeleteOrderShippingResponse(bool Deleted);


public record CreateOrderRequest(int CartId, int PaymentStatusId, int OrderShippingId, int PaymentVendorTypeId, string Comment);
public record CreateOrderResponse(int OrderId);

public record UpdateOrderStatusRequest(int OrderId, int StatusId);
public record UpdateOrderResponse(bool Updated);