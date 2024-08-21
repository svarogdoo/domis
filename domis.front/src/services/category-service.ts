import { fetchData } from "../helpers/fetch";
import { categories } from "../stores/categories";

export function getCategories() {
  return fetchData<Array<Category>>(
    "https://domis.onrender.com/api/categories"
  );
}

export function getCategoryProducts(id: number) {
  return fetchData<Array<Category>>(
    `https://domis.onrender.com/api/products/category/${id}`
  );
}

export async function setCategories() {
  categories.subscribe(async (value) => {
    if (value.length === 0) {
      categories.set(await getCategories());
    }
  });
}
