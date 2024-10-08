import { fetchDataWithJsonBody } from "../helpers/fetch";


export async function login(email: string, password: string): Promise<UserLoginResponse> {
    const loginResponse = await fetchDataWithJsonBody<UserLoginResponse>(
        'https://domis.onrender.com/login',
        'POST',
        JSON.stringify({ email, password })
        );

    return loginResponse;
}