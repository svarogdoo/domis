import { writable } from "svelte/store";
import {
  addCartItem,
  getCart,
  removeCartItem,
  updateCartItem,
} from "../services/cart-service";

function createCart() {
  const { subscribe, set, update } = writable<Cart>();
  return {
    subscribe,
    initialize: async () => {
      set(await getCart());
    },
    add: async (product: CartProductDto) => {
      await addCartItem(product);
      set(await getCart());
    },
    remove: async (cartItemId: number) => {
      await removeCartItem(cartItemId);
      set(await getCart());
    },
    update: async (cartItemId: number, quantity: number) => {
      await updateCartItem({ cartItemId: cartItemId, quantity: quantity });
      set(await getCart());
    },
  };
}

export const cart = createCart();
