import { get, writable } from "svelte/store";
import {
  addCartItem,
  getCart,
  removeCartItem,
  updateCartItem,
} from "../services/cart-service";

function createCart() {
  const { subscribe, set, update } = writable<Cart>();

  const getCartId = () => {
    const currentCart = get(cart);
    if (currentCart?.cartId) return currentCart.cartId;
    else return undefined;
  };

  return {
    subscribe,
    initialize: async () => {
      const cart = await getCart(getCartId());
      if (cart) set(cart);
    },
    add: async (product: CartProductDto) => {
      product.cartId = getCartId();
      const cartItemResponse = await addCartItem(product);
      update((currentCart) => ({
        ...currentCart,
        cartId: cartItemResponse.cartId || currentCart?.cartId,
      }));

      cart.initialize();
    },
    remove: async (cartItemId: number) => {
      await removeCartItem(cartItemId);
      cart.initialize();
    },
    update: async (cartItemId: number, quantity: number) => {
      await updateCartItem({ cartItemId: cartItemId, quantity: quantity });
      cart.initialize();
    },
  };
}

export const cart = createCart();
