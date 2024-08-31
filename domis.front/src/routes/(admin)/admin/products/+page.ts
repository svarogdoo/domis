import { setCategories } from "../../../../services/category-service";

export async function load() {
  setCategories();
}
