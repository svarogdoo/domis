import { API_URL } from "../config";
import { fetchDataWithJsonBody } from "../helpers/fetch";

export function saveShippingDetails(shippingDetails: ShippingDetails) {
  return fetchDataWithJsonBody<ShippingId>(
    `${API_URL}/api/order/shipping`,
    "post",
    JSON.stringify(shippingDetails)
  );
}

// post ceo order
