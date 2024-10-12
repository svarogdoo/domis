import { fetchData, fetchDataWithJsonBody, setAuthToken } from "../helpers/fetch";
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

async function profile() : Promise<UserProfile> {
    const profile = await fetchData<UserProfile>(
        `${API_URL}/api/user/profile`,
        'GET'
    );
    
    return profile;
}

export const userService = {
    login,
    register,
    profile
};