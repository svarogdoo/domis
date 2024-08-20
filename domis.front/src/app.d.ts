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
  interface Category {
    categoryId: string;
    categoryName: string;
    subcategories?: Array<Category>;
  }
}

export {};
