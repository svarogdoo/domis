import {
  fetchData,
  fetchDataWithJsonBody,
  setAuthToken,
} from "../helpers/fetch";
import { API_URL } from "../config";

async function login(
  email: string,
  password: string
): Promise<UserLoginResponse> {
  const loginResponse = await fetchDataWithJsonBody<UserLoginResponse>(
    `${API_URL}/login`,
    "POST",
    JSON.stringify({ email, password })
  );

  setAuthToken(loginResponse.accessToken);

  return loginResponse;
}

async function register(body: UserRegisterRequest): Promise<void> {
  await fetchDataWithJsonBody<void>(
    `${API_URL}/register`,
    "POST",
    JSON.stringify(body)
  );
}

async function getProfile(): Promise<UserProfileResponse> {
  const profile = await fetchData<UserProfileResponse>(
    `${API_URL}/api/user/profile`,
    "GET"
  );

  return profile;
}

async function updateProfile(body: UserProfileUpdateRequest): Promise<void> {
  await fetchDataWithJsonBody<void>(
    `${API_URL}/api/user/profile`,
    "PUT",
    JSON.stringify(body)
  );
}

async function forgotPassword(email: string): Promise<void> {
  await fetchDataWithJsonBody<void>(
    `${API_URL}/forgotPassword`,
    "POST",
    JSON.stringify({ email })
  );
}

async function resetPassword(
  email: string,
  resetCode: string,
  newPassword: string
) {
  await fetchDataWithJsonBody<void>(
    `${API_URL}/resetPassword`,
    "POST",
    JSON.stringify({ email, resetCode, newPassword })
  );
}

async function refreshAccessToken(
  refreshToken: string
): Promise<UserLoginResponse> {
  const result = await fetchDataWithJsonBody<UserLoginResponse>(
    `${API_URL}/refresh`,
    "POST",
    JSON.stringify({ refreshToken })
  );

  return result;
}

async function getUserOrders() {
  return await fetchData<Array<UserOrder>>(
    `${API_URL}/api/user/orders`,
    "GET"
  ).catch(() => []);
}

async function getUserRole() {
  return await fetchData<string>(`${API_URL}/api/user/role`, "GET");
}

export const userService = {
  login,
  register,
  getProfile,
  updateProfile,
  forgotPassword,
  resetPassword,
  refreshAccessToken,
  getUserOrders,
  getUserRole,
};
