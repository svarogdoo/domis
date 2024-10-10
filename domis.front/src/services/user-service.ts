import { fetchDataWithJsonBody, setAuthToken } from "../helpers/fetch";

async function login(email: string, password: string): Promise<UserLoginResponse> {
    const loginResponse = await fetchDataWithJsonBody<UserLoginResponse>(
        'https://domis.onrender.com/login',
        'POST',
        JSON.stringify({ email, password })
        );

    setAuthToken(loginResponse.accessToken);

    return loginResponse;
}

async function register(email: string, password: string): Promise<void> {
    await fetchDataWithJsonBody<void>(
        'https://domis.onrender.com/register',
        'POST',
        JSON.stringify({ email, password })
    );
}

export const userService = {
    login,
    register
};