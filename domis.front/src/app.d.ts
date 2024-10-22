// See https://kit.svelte.dev/docs/types#app

import type { OrderStatus, PaymentVendorType, QuantityType } from "./enums";
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

  interface UserCreds {
    userId?: number;
    userName: string;
    tokenType: string;
    accessToken: string;
    expiresIn: number;
    refreshToken: string;
  }
  interface UserRegisterRequest {
    firstName: string;
    lastName: string;
    email: string;
    password: string;
  }
  interface UserLoginResponse {
    tokenType: string;
    accessToken: string;
    expiresIn: number;
    refreshToken: string;
  }
  interface UserProfileResponse {
    id: number;
    email: string;
    firstName: string;
    lastName: string;
    addressLine: string;
    city: string;
    zipCode: string;
    country: string;
    phoneNumber: string;
  }
  interface UserProfileUpdateRequest {
    firstName: string;
    lastName: string;
    addressLine: string;
    city: string;
    zipCode: string;
    country: string;
    phoneNumber: string;
  }
  interface UserState {
    isAuthenticated: boolean;
    user: UserProfileResponse | null;
    token: string | null;
    refreshToken: string | null;
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
  interface ProductBasicInfo {
    id: number;
    name: string;
    sku: number;
  }

  interface Cart {
    cartId?: number;
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
    productDetails: ProductDetails;
  }
  interface ProductDetails {
    sku: number;
    name: string;
    image: string;
    // price: number;
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
  interface CartItemResponse {
    cartId: number;
  }

  interface CheckoutFormData {
    shippingDetails: ShippingDetails;
    comment: string;
  }
  interface ShippingDetails {
    firstName: string;
    lastName: string;
    companyName: string;
    countryId: number;
    city: string;
    address: string;
    apartment: string;
    county: string;
    postalCode: string;
    phoneNumber: string;
    email: string;
  }
  interface ShippingResponse {
    orderShippingId: number;
  }

  interface Order {
    cartId: number;
    paymentStatusId;
    orderShippingId: number;
    paymentVendorTypeId;
    comment: string;
  }
  interface OrderResponse {
    orderId: number;
  }
  interface UserOrder {
    id: number;
    date: string;
    statusId: OrderStatus;
    address: string;
    paymentTypeId: PaymentVendorType;
    paymentAmount: number;
    orderItems: Array<UserOrderItem>;
    comment: string;
  }
  interface UserOrderItem {
    id: number;
    quantity: number;
    itemPrice: number;
    itemPriceTotal: number;
    productDetails: ProductDetails;
  }

  interface SalesPoint {
    id: number;
    name: string;
    address: string;
    phoneNumbers: string[];
    workingHours: string;
    image?: string;
    googleMapPin?: string;
    optionalInfo?: string;
  }
}

export {};
