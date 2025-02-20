import {
  fetchData,
  fetchDataWithJsonBody,
  setAuthToken,
} from "../helpers/fetch";
import { PUBLIC_API_URL } from "$env/static/public";

async function login(
  email: string,
  password: string
): Promise<UserLoginResponse> {
  const loginResponse = await fetchDataWithJsonBody<UserLoginResponse>(
    `${PUBLIC_API_URL}/login`,
    "POST",
    JSON.stringify({ email, password })
  );

  setAuthToken(loginResponse.accessToken);

  return loginResponse;
}

async function register(body: UserRegisterRequest): Promise<void> {
  await fetchDataWithJsonBody<void>(
    `${PUBLIC_API_URL}/register`,
    "POST",
    JSON.stringify(body)
  );
}

async function getProfile(): Promise<UserProfileResponse> {
  const profile = await fetchData<UserProfileResponse>(
    `${PUBLIC_API_URL}/api/user/profile`,
    "GET"
  );

  return profile;
}

async function updateProfile(body: UserProfileUpdateRequest): Promise<void> {
  await fetchDataWithJsonBody<void>(
    `${PUBLIC_API_URL}/api/user/profile`,
    "PUT",
    JSON.stringify(body)
  );
}

async function forgotPassword(email: string): Promise<void> {
  await fetchDataWithJsonBody<void>(
    `${PUBLIC_API_URL}/forgotPassword`,
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
    `${PUBLIC_API_URL}/resetPassword`,
    "POST",
    JSON.stringify({ email, resetCode, newPassword })
  );
}

async function refreshAccessToken(
  refreshToken: string
): Promise<UserLoginResponse> {
  const result = await fetchDataWithJsonBody<UserLoginResponse>(
    `${PUBLIC_API_URL}/refresh`,
    "POST",
    JSON.stringify({ refreshToken })
  );

  return result;
}

async function getUserOrders() {
  return await fetchData<Array<UserOrder>>(
    `${PUBLIC_API_URL}/api/user/orders`,
    "GET"
  ).catch(() => []);
}

async function getUserRole() {
  return await fetchData<string>(`${PUBLIC_API_URL}/api/user/role`, "GET");
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
