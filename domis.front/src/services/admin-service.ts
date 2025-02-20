import { fetchData, putDataWithJsonBody } from "../helpers/fetch";
import { PUBLIC_API_URL } from "$env/static/public";

export function getAdminUsers() {
  return fetchData<Array<AdminUser>>(`${PUBLIC_API_URL}/api/admin/users`);
}

export function updateUserRole(userId: string, role: string) {
  return putDataWithJsonBody(
    `${PUBLIC_API_URL}/api/admin/user-role/${userId}`,
    JSON.stringify({ role: role })
  ).catch((error) => false);
}

export function getProductSaleHistory(productId: number) {
  return fetchData<Array<SaleInfo>>(
    `${PUBLIC_API_URL}/api/admin/product/${productId}/sale-history`
  ).catch((error) => []);
}

export function getAdminOrders() {
  return fetchData<Array<AdminOrder>>(`${PUBLIC_API_URL}/api/admin/orders`);
}
