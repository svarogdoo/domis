import {
  deleteData,
  fetchData,
  fetchDataWithJsonBody,
  putDataWithJsonBody,
} from "../helpers/fetch";
import { API_URL } from "../config";

export function createCat(user: CartUser) {
  return fetchDataWithJsonBody<Cart>(
    `${API_URL}/api/cart`,
    "post",
    JSON.stringify(user)
  );
}

export async function getCart(cartId?: number) {
  try {
    if (cartId)
      return await fetchData<Cart>(`${API_URL}/api/cart?cartId=${cartId}`);
    else return await fetchData<Cart>(`${API_URL}/api/cart`);
  } catch {
    return null;
  }
}

export function addCartItem(cartProduct: CartProductDto) {
  let cartProductDto: CartProductDto = {
    cartId: cartProduct.cartId,
    productId: cartProduct.productId,
    quantity: cartProduct.quantity,
  };
  return fetchDataWithJsonBody<CartItemResponse>(
    `${API_URL}/api/cart/cart-item`,
    "post",
    JSON.stringify(cartProductDto)
  ).catch((error) => null);
}

export function removeCartItem(cartItemId: number) {
  return deleteData(`${API_URL}/api/cart/cart-item/${cartItemId}`);
}

export function updateCartItem(cartItemQuantity: CartItemQuantityUpdateDto) {
  return putDataWithJsonBody(
    `${API_URL}/api/cart/cart-item-quantity`,
    JSON.stringify(cartItemQuantity)
  );
}
