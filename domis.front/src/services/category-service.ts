import { fetchData } from "../helpers/fetch";
import { categories } from "../stores/categories";
import { API_URL } from "../config";

export function getCategories() {
  try {
    return fetchData<Array<Category>>(`${API_URL}/api/categories`);
  } catch {
    console.info("getCategories fail");
    return [];
  }
}

export function getCategoryProducts(id: number, pageNumber?: number, pageSize?: number) {
  let queryParams = new URLSearchParams();
  if (pageNumber) queryParams.append("pageNumber", pageNumber.toString());
  if (pageSize) queryParams.append("pageSize", pageSize.toString());

  const url = `${API_URL}/api/categories/${id}/products?${queryParams.toString()}`;

  return fetchData<CategoryData>(url);
}

export async function setCategories() {
  categories.subscribe(async (value) => {
    if (value?.length === 0) {
      categories.set(await getCategories());
    }
  });
}
