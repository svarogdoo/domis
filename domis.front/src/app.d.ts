// See https://kit.svelte.dev/docs/types#app
// for information about these interfaces
declare global {
  namespace App {
    // interface Error {}
    // interface Locals {}
    // interface PageData {}
    // interface PageState {}
    // interface Platform {}
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
    featuredImageUrl?: string;
  }

  interface Product {
    id: number;
    name: string;
    description?: string;
    sku: number;
    price: number;
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
  }
}

export {};
