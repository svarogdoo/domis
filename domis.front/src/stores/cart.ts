import { writable, type Writable } from "svelte/store";

function createCart() {
  const { subscribe, set, update } = writable<CartProduct[]>([]);
  return {
    subscribe,
    add: (product: CartProduct) =>
      update((currentCart) => {
        const existingProductIndex = currentCart.findIndex(
          (p) => p.id === product.id
        );

        if (existingProductIndex !== -1)
          currentCart[existingProductIndex].quantity += product.quantity;
        else currentCart.push(product);

        return currentCart;
      }),
    remove: (productId: number) =>
      update((currentCart) => {
        return currentCart.filter((product) => product.id !== productId);
      }),
    empty: () => set([]),
  };
}

export const cart = createCart();
