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
  interface CategoryProduct {
    name: string;
    sku: string;
    price: number;
    stock: number;
    featuredImageUrl?: string;
  }

  interface Product {
    name: string;
    description: string;
    sku: string;
    price: number;
    stock: number;
    images: Array<Image>;
    categoryPaths: Array<string>;
  }
}

export {};
