import { get, writable } from "svelte/store";
import {
  addCartItem,
  getCart,
  removeCartItem,
  updateCartItem,
} from "../services/cart-service";

function createCart() {
  const { subscribe, set } = writable<Cart>();

  const getCartId = () => {
    const currentCart = get(cart);
    if (currentCart?.cartId) return currentCart.cartId;
    else return undefined;
  };

  return {
    subscribe,
    initialize: async () => {
      set(await getCart(getCartId()));
    },
    add: async (product: CartProductDto) => {
      product.cartId = getCartId();
      await addCartItem(product);
      set(await getCart(getCartId()));
    },
    remove: async (cartItemId: number) => {
      await removeCartItem(cartItemId);
      set(await getCart(getCartId()));
    },
    update: async (cartItemId: number, quantity: number) => {
      await updateCartItem({ cartItemId: cartItemId, quantity: quantity });
      set(await getCart(getCartId()));
    },
  };
}

export const cart = createCart();
