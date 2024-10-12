import { fetchData } from "../helpers/fetch";
import { categories } from "../stores/categories";

export function getCategories() {
  try {
    return fetchData<Array<Category>>(
      "https://domis.onrender.com/api/categories"
    );
  } catch {
    console.info("getCategories fail");
    return [];
  }
}

export function getCategoryProducts(id: number) {
  return fetchData<CategoryData>(
    `https://domis.onrender.com/api/categories/${id}/products`
  );
}

export async function setCategories() {
  categories.subscribe(async (value) => {
    if (value?.length === 0) {
      categories.set(await getCategories());
    }
  });
}
