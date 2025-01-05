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
  sortType?: SortType,
  minPrice?: string,
  maxPrice?: string,
  minWidth?: string,
  maxWidth?: string,
  minHeight?: string,
  maxHeight?: string
) {
  let queryParams = new URLSearchParams();
  if (pageNumber) queryParams.append("pageNumber", pageNumber.toString());
  if (pageSize) queryParams.append("pageSize", pageSize.toString());
  if (sortType) queryParams.append("sort", sortType.toString());
  if (minPrice) queryParams.append("minPrice", minPrice.toString());
  if (maxPrice) queryParams.append("maxPrice", maxPrice.toString());
  if (minWidth) queryParams.append("minWidth", minWidth.toString());
  if (maxWidth) queryParams.append("maxWidth", maxWidth.toString());
  if (minHeight) queryParams.append("minHeight", minHeight.toString());
  if (maxHeight) queryParams.append("maxHeight", maxHeight.toString());

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
