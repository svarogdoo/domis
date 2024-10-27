import { fetchDataWithJsonBody } from "../helpers/fetch";
import { API_URL } from "../config";

async function subscribeToNewsletter(email: string) {
    await fetchDataWithJsonBody(`${API_URL}/api/sync/newsletter`, 
        'POST',
        JSON.stringify(email)
    );
}

export const syncService = {
    subscribeToNewsletter
}