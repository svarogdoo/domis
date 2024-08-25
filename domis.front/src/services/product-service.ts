import { fetchData } from "../helpers/fetch";

export function getProduct(id: number) {
  return fetchData<Product>(`https://domis.onrender.com/api/products/${id}`);
}

export function getProducts() {
  return fetchData<Array<Product>>("https://domis.onrender.com/api/products");
}
