import { setCategories } from "../services/category-service";
import { userStore } from "../stores/user";

export const ssr = false;

export async function load() {
  userStore.initialize();
  setCategories();
}
