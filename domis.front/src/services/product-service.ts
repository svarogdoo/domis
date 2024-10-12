import { fetchData } from "../helpers/fetch";
import { API_URL } from "../config";

export function getProduct(id: number) {
  return fetchData<Product>(`${API_URL}/api/products/${id}`);
}

export function getProducts() {
  return fetchData<Array<Product>>(`${API_URL}/api/products`);
}

export async function putProduct(product: any) {
  try {
    const response = await fetch(`${API_URL}/api/products`, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(product),
    });

    if (!response.ok) {
      throw new Error(`HTTP error! status: ${response.status}`);
    }

    return true;
  } catch (error) {
    console.error("There was a problem with the PUT request:", error);
    throw error;
  }
}

export async function getCategoryProductsBasicInfo(categoryId: string) {
  return fetchData<Array<Product>>(
    `${API_URL}/api/products/basic-info?categoryId=${categoryId}`
  );
}
