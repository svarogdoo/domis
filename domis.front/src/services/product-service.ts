import {
  deleteData,
  fetchData,
  fetchDataWithJsonBody,
  putDataWithJsonBody,
} from "../helpers/fetch";
import { PUBLIC_API_URL } from "$env/static/public";

export function getProduct(id: number) {
  return fetchData<Product>(`${PUBLIC_API_URL}/api/products/${id}`);
}

export function getProducts() {
  return fetchData<Array<Product>>(`${PUBLIC_API_URL}/api/products`);
}

export async function searchProductsOrCategories(searchTerm: string) {
  const url = `${PUBLIC_API_URL}/api/products/search?searchTerm=${encodeURIComponent(
    searchTerm
  )}`;
  return fetchData<Array<SearchResult>>(url, "GET");
}

export async function putProduct(product: any) {
  return putDataWithJsonBody(
    `${PUBLIC_API_URL}/api/products`,
    JSON.stringify(product)
  ).catch((error) => false);
}

export async function postProduct(newProduct: NewProduct) {
  return fetchDataWithJsonBody(
    `${PUBLIC_API_URL}/api/admin/product`,
    "POST",
    JSON.stringify(newProduct)
  ).catch((error) => error.errorMessage ?? false);
}

export async function getCategoryProductsBasicInfo(categoryId: string) {
  return fetchData<Array<Product>>(
    `${PUBLIC_API_URL}/api/products/basic-info?categoryId=${categoryId}`
  );
}

export async function postProductOnSale(saleInfo: any) {
  return fetchDataWithJsonBody(
    `${PUBLIC_API_URL}/api/admin/product/sale`,
    "POST",
    JSON.stringify(saleInfo)
  ).catch(() => false);
}

export async function deactivateProductSale(productIdArray: Array<number>) {
  return fetchDataWithJsonBody(
    `${PUBLIC_API_URL}/api/admin/product/sale`,
    "DELETE",
    JSON.stringify(productIdArray)
  ).catch(() => false);
}

export async function postProductGalleryImages(
  productId: number,
  imagesArray: ImageUpload
) {
  return fetchDataWithJsonBody(
    `${PUBLIC_API_URL}/api/admin/images/${productId}/gallery-images`,
    "POST",
    JSON.stringify(imagesArray)
  ).catch(() => false);
}

export async function deleteProductGalleryImages(
  imageId: number,
  productId: number
) {
  return deleteData(
    `${PUBLIC_API_URL}/api/admin/images/${imageId}/products/${productId}`
  )
    .then((res) => true)
    .catch(() => false);
}

export async function upsertFeaturedImage(
  productId: number,
  imageString: string
) {
  return putDataWithJsonBody(
    `${PUBLIC_API_URL}/api/admin/images/${productId}/featured-image`,
    JSON.stringify(imageString)
  )
    .then((res) => true)
    .catch(() => false);
}

export const productService = {
  getProduct,
  getProducts,
  searchProductsOrCategories,
  putProduct,
  getCategoryProductsBasicInfo,
};
