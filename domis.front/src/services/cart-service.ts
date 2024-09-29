import {
  deleteData,
  fetchData,
  fetchDataWithJsonBody,
  putDataWithJsonBody,
} from "../helpers/fetch";
import { cart } from "../stores/cart";

let cartId: number = 3;
// cart.subscribe((value) => {
//   cartId = value.cartId;
// });

export function createCat(user: CartUser) {
  return fetchDataWithJsonBody<Cart>(
    "https://domis.onrender.com/api/cart",
    "post",
    JSON.stringify(user)
  );
}

export function getCart() {
  return fetchData<Cart>(`https://domis.onrender.com/api/cart/${cartId}`);
}

export function addCartItem(cartProduct: CartProductDto) {
  let cartProductDto: CartProductDto = {
    cartId: cartId,
    productId: cartProduct.productId,
    quantity: cartProduct.quantity,
  };
  return fetchDataWithJsonBody<CartProductDto>(
    "https://domis.onrender.com/api/cart/cart-item",
    "post",
    JSON.stringify(cartProductDto)
  );
}

export function removeCartItem(cartItemId: number) {
  return deleteData(
    `https://domis.onrender.com/api/cart/cart-item/${cartItemId}`
  );
}

export function updateCartItem(cartItemQuantity: CartItemQuantityUpdateDto) {
  return putDataWithJsonBody(
    "https://domis.onrender.com/api/cart/cart-item-quantity",
    JSON.stringify(cartItemQuantity)
  );
}
