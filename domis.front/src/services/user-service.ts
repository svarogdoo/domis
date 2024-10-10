import { fetchDataWithJsonBody, setAuthToken } from "../helpers/fetch";
import { API_URL } from "../config";

async function login(email: string, password: string): Promise<UserLoginResponse> {
    const loginResponse = await fetchDataWithJsonBody<UserLoginResponse>(
        `${API_URL}/login`,
        'POST',
        JSON.stringify({ email, password })
        );

    setAuthToken(loginResponse.accessToken);

    return loginResponse;
}

async function register(email: string, password: string): Promise<void> {
    await fetchDataWithJsonBody<void>(
        `${API_URL}/register`,
        'POST',
        JSON.stringify({ email, password })
    );
}

export const userService = {
    login,
    register
};