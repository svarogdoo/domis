// See https://kit.svelte.dev/docs/types#app

import type {
  OrderStatus,
  PaymentVendorType,
  QuantityType,
  UserRole,
} from "./enums";
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
  interface UserProfileDto {
    firstName: string;
    lastName: string;
    addressLine?: string;
    apartment?: string;
    city?: string;
    postalCode?: string;
    country?: string;
    county?: string;
    email: string;
    phoneNumber?: string;
    companyInfo?: CompanyData;
  }
  interface UserProfileResponse {
    firstName: string;
    lastName: string;
    email: string;
    phoneNumber: string;
    companyInfo?: CompanyInfo;
    addressInvoice: Address;
    addressDelivery: Address;
    useSameAddress: boolean;
  }

  interface Address {
    country: string;
    county: string;
    city: string;
    addressLine: string;
    apartment: string;
    postalCode: string;
    contactPerson: string;
    contactPhone: string;
  }
  interface CompanyInfo {
    name: string;
    number: string;
    firstName: string;
    lastName: string;
  }

  interface UserProfileUpdateRequest {
    firstName: string;
    lastName: string;
    addressLine: string;
    apartment: string;
    city: string;
    postalCode: string;
    country: string;
    county: string;
    phoneNumber: string;
  }
  interface UserState {
    isAuthenticated: boolean;
    user: UserProfileResponse | null;
    token: string | null;
    refreshToken: string | null;
    userRole: UserRole;
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
    vpPrice: number;
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
    vpPrice: ProductPricing;
    stock: number;
    size: ProductSizing;
    featuredImageUrl: string;
    images: Array<Image>; // ne slati za put
    categoryPaths: Array<Array<CategoryPath>>;
    attributes: ProductAttributes;
    isActive?: boolean;
  }
  interface ProductPricing {
    perUnit?: number;
    perBox?: number;
    perPallet?: number;
    perPalletUnit?: number;
  }
  interface ProductSizing {
    box: number;
    pallet: number | null;
  }
  interface ProductBasicInfo {
    id: number;
    name: string;
    sku: number;
  }
  interface CategoryPath {
    id: number;
    name: string;
  }
  interface ProductAttributes {
    quantityType?: QuantityType;
    title?: string;
    width?: number;
    height?: number;
    depth?: number;
    length?: number;
    thickness?: number;
    weight?: number;
  }

  interface SearchResult {
    id: number;
    name: string;
    sku: number;
    type: string;
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
    addressInvoice: ShippingDetails;
    addressDelivery: ShippingDetails | null;
    companyInfo: CompanyInfo | null;
    comment: string;
  }
  interface ShippingDetails {
    firstName?: string;
    lastName?: string;
    phoneNumber?: string;
    email?: string;
    companyInfo?: CompanyInfo;
    countryId: number;
    city: string;
    addressLine: string;
    apartment: string;
    county: string;
    postalCode: string;
    contactPhone?: string;
    contactPerson?: string;
    addressType: AddressType;
  }
  interface ShippingDetailsRequest {
    addressInvoice: ShippingDetails;
    addressDelivery: ShippingDetails | null;
    companyInfo: CompanyInfo | null;
  }
  interface ShippingResponse {
    invoiceId: number;
    deliveryId: number | null;
  }

  interface Order {
    cartId: number;
    paymentStatusId;
    invoiceOrderShippingId: number;
    deliveryOrderShippingId: number | null;
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

  interface AdminUser {
    userId: string;
    firstName: string;
    lastName: string;
    email: string;
    role: string;
    userName: string;
  }
}

export {};
