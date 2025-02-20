import { PUBLIC_API_URL } from "$env/static/public";
import { fetchDataWithJsonBody } from "../helpers/fetch";

export function saveShippingDetails(shippingDetails: ShippingDetailsRequest) {
  try {
    return fetchDataWithJsonBody<ShippingResponse>(
      `${PUBLIC_API_URL}/api/order/shipping`,
      "post",
      JSON.stringify(shippingDetails)
    );
  } catch {
    return null;
  }
}

export function saveOrder(order: Order) {
  return fetchDataWithJsonBody<OrderResponse>(
    `${PUBLIC_API_URL}/api/order`,
    "post",
    JSON.stringify(order)
  ).catch(() => false);
}
