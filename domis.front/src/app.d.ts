// See https://kit.svelte.dev/docs/types#app

import type { QuantityType } from "./enums";
import type CartItem from "./routes/korpa/CartItem.svelte";

// for information about these interfaces
declare global {
  namespace App {
    // interface Error {}
    // interface Locals {}
    // interface PageData {}
    // interface PageState {}
    // interface Platform {}
  }

  interface User {
    userId: number;
    userName: string;
    tokenType: string;
    accessToken: string;
    expiresIn: number;
    refreshToken: string;
  }

  interface Image {
    url: string;
    type: string;
  }

  interface Category {
    id: string;
    name: string;
    subcategories?: Array<Category>;
  }
  interface CategoryData {
    category: CategoryDetails;
    products: Array<CategoryProduct>;
  }
  interface CategoryDetails {
    id: string;
    name: string;
    description: string;
  }
  interface CategoryProduct {
    id: string;
    name: string;
    sku: string;
    price: number;
    stock: number;
    quantityType?: QuantityType;
    featuredImageUrl?: string;
  }

  interface Product {
    id: number;
    name: string;
    description?: string;
    sku: number;
    price: ProductPricing;
    size: ProductSizing;
    stock: number;
    featuredImageUrl: string;
    images: Array<Image>; // ne slati za put
    categoryPaths: Array<string>; // ne slati za put
    title?: string;
    width?: number;
    height?: number;
    depth?: number;
    length?: number;
    thickness?: number;
    weight?: number;
    isItemType?: boolean;
    isSurfaceType?: boolean;
    isActive?: boolean;
    quantityType?: QuantityType;
  }
  interface ProductPricing {
    perUnit?: number;
    perBox?: number;
    perPallet?: number;
  }
  interface ProductSizing {
    box: number;
    pallet: number;
  }

  interface Cart {
    cartId: number;
    userId: number;
    statusId: number;
    status: string;
    createdAt: Date;
    items: Array<CartProduct>;
    totalCartPrice: number;
  }
  interface CartUser {
    userId: number;
  }
  interface CartProduct {
    cartItemId: number;
    productId: number;
    quantity: number;
    cartItemPrice: number;
    productDetails: CartProductDetails;
  }
  interface CartProductDetails {
    sku: number;
    name: string;
    image: string;
    price: number;
    quantityType: QuantityType;
  }
  interface CartProductDto {
    cartId?: number;
    productId: number;
    quantity: number;
  }
  interface CartItemQuantityUpdateDto {
    cartItemId: number;
    quantity: number;
  }
}

export {};
