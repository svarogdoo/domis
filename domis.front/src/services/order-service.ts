import { API_URL } from "../config";
import { fetchDataWithJsonBody } from "../helpers/fetch";

export function saveShippingDetails(shippingDetails: ShippingDetails) {
  try {
    return fetchDataWithJsonBody<ShippingResponse>(
      `${API_URL}/api/order/shipping`,
      "post",
      JSON.stringify(shippingDetails)
    );
  } catch {
    return null;
  }
}

export function saveOrder(order: Order) {
  try {
    return fetchDataWithJsonBody<OrderResponse>(
      `${API_URL}/api/order`,
      "post",
      JSON.stringify(order)
    );
  } catch {
    return null;
  }
}
