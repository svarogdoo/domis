import { fetchData, putDataWithJsonBody } from "../helpers/fetch";
import { API_URL } from "../config";

export function getAdminUsers() {
  return fetchData<Array<AdminUser>>(`${API_URL}/api/admin/users`);
}

export function updateUserRole(userId: string, role: string) {
  return putDataWithJsonBody(
    `${API_URL}/api/admin/user-role/${userId}`,
    JSON.stringify({ role: role })
  ).catch((error) => {
    return false;
  });
}
