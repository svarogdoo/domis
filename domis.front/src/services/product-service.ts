import { fetchData } from "../helpers/fetch";

export function getProduct(id: number) {
  return fetchData<Product>(`https://domis.onrender.com/api/products/${id}`);
}

export function getProducts() {
  return fetchData<Array<Product>>("https://domis.onrender.com/api/products");
}

export async function putProduct(product: any) {
  try {
    const response = await fetch("https://domis.onrender.com/api/products", {
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
