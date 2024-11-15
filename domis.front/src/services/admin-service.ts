import { fetchData } from "../helpers/fetch";
import { API_URL } from "../config";

export function getAdminUsers() {
  return fetchData<Array<AdminUser>>(`${API_URL}/api/admin/users`);
}
