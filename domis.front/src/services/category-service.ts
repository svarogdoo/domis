import { fetchData } from "../helpers/fetch";
import { categories } from "../stores/categories";
import { API_URL } from "../config";
import type { SortType } from "../enums";

export function getCategories() {
  try {
    return fetchData<Array<Category>>(`${API_URL}/api/categories`);
  } catch {
    return [];
  }
}

export function getCategoryProducts(
  id: number | string,
  pageNumber?: number,
  pageSize?: number,
  sortType?: SortType
) {
  console.info(id, "ajd");
  let queryParams = new URLSearchParams();
  if (pageNumber) queryParams.append("pageNumber", pageNumber.toString());
  if (pageSize) queryParams.append("pageSize", pageSize.toString());
  if (sortType) queryParams.append("sort", sortType.toString());

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
