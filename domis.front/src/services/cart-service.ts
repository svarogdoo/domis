import {
  deleteData,
  fetchData,
  fetchDataWithJsonBody,
  putDataWithJsonBody,
} from "../helpers/fetch";
import { cart } from "../stores/cart";
import { API_URL } from "../config"

let cartId: number = 3;
// cart.subscribe((value) => {
//   cartId = value.cartId;
// });

export function createCat(user: CartUser) {
  return fetchDataWithJsonBody<Cart>(
    `${API_URL}/api/cart`,
    "post",
    JSON.stringify(user)
  );
}

export function getCart() {
  return fetchData<Cart>(`${API_URL}/api/cart/${cartId}`);
}

export function addCartItem(cartProduct: CartProductDto) {
  let cartProductDto: CartProductDto = {
    cartId: cartId,
    productId: cartProduct.productId,
    quantity: cartProduct.quantity,
  };
  return fetchDataWithJsonBody<CartProductDto>(
    `${API_URL}/api/cart/cart-item`,
    "post",
    JSON.stringify(cartProductDto)
  );
}

export function removeCartItem(cartItemId: number) {
  return deleteData(
    `${API_URL}/api/cart/cart-item/${cartItemId}`
  );
}

export function updateCartItem(cartItemQuantity: CartItemQuantityUpdateDto) {
  return putDataWithJsonBody(
    `${API_URL}/api/cart/cart-item-quantity`,
    JSON.stringify(cartItemQuantity)
  );
}
