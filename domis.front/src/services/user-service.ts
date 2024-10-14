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

async function register(body: UserRegisterRequest): Promise<void> {
    await fetchDataWithJsonBody<void>(
        `${API_URL}/register`,
        'POST',
        JSON.stringify(body)
    );
}

async function getProfile() : Promise<UserProfileResponse> {
    const profile = await fetchData<UserProfileResponse>(
        `${API_URL}/api/user/profile`,
        'GET'
    );
    
    return profile;
}

async function updateProfile(body: UserProfileUpdateRequest): Promise<void> {
    await fetchDataWithJsonBody<void>(
        `${API_URL}/api/user/profile`,
        'PUT',
        JSON.stringify(body)
    );
}

async function forgotPassword(email: string): Promise<void> {
    await fetchDataWithJsonBody<void>(
        `${API_URL}/forgotPassword`,
        'POST',
        JSON.stringify({ email })
    );
}

async function resetPassword(email: string, resetCode: string, newPassword: string){
    await fetchDataWithJsonBody<void>(
        `${API_URL}/resetPassword`,
        'POST',
        JSON.stringify({ email, resetCode, newPassword })
    );
}

export const userService = {
    login,
    register,
    getProfile,
    updateProfile,
    forgotPassword,
    resetPassword
};