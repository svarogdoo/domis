import { get, writable } from "svelte/store";
import {
  addCartItem,
  getCart,
  removeCartItem,
  updateCartItem,
} from "../services/cart-service";

function createCart() {
  const { subscribe, set, update } = writable<Cart | null>(null);

  const getCartId = () => {
    const currentCart = get(cart);
    if (currentCart?.cartId) return currentCart.cartId;
    else return undefined;
  };

  const setLocalStorageCartId = () => {
    localStorage.setItem("cart", JSON.stringify({ cartId: getCartId() }));
  };

  const removeCartFromLocalStorage = () => {
    localStorage.setItem("cart", JSON.stringify({ cartId: null }));
  };

  return {
    subscribe,
    initialize: async () => {
      const savedCart = localStorage.getItem("cart");
      if (savedCart) {
        let cartParsed = JSON.parse(savedCart);
        if (cartParsed.cartId)
          set({ ...cartParsed, cartId: cartParsed.cartId });
      }
      await cart.get();
    },
    loginUser: async () => {
      removeCartFromLocalStorage();
      set(null);
      await cart.get();
      setLocalStorageCartId();
    },
    logoutUser: async () => {
      removeCartFromLocalStorage();
      set(null);
    },
    get: async (cartId?: number) => {
      const cartFetch = cartId
        ? await getCart(cartId)
        : await getCart(getCartId());
      set(cartFetch);
    },
    add: async (product: CartProductDto) => {
      product.cartId = getCartId();
      const cartItemResponse = await addCartItem(product);

      if (cartItemResponse && cartItemResponse.cartId)
        await cart.get(cartItemResponse.cartId);
      else await cart.get();
      setLocalStorageCartId();

      return cartItemResponse;
    },
    remove: async (cartItemId: number) => {
      await removeCartItem(cartItemId);
      await cart.get();
    },
    update: async (cartItemId: number, quantity: number) => {
      await updateCartItem({ cartItemId: cartItemId, quantity: quantity });
      await cart.get();
    },
    reset: async () => {
      removeCartFromLocalStorage();
      set(null);
      await cart.get();
      setLocalStorageCartId();
    },
  };
}

export const cart = createCart();
