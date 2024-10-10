import { fetchData } from "../helpers/fetch";
import { categories } from "../stores/categories";
import { API_URL } from "../config";
export function getCategories() {
  return fetchData<Array<Category>>(
    `${API_URL}/api/categories`
  );
}

export function getCategoryProducts(id: number) {
  return fetchData<CategoryData>(
    `${API_URL}/api/categories/${id}/products`
  );
}

export async function setCategories() {
  categories.subscribe(async (value) => {
    if (value?.length === 0) {
      categories.set(await getCategories());
    }
  });
}
