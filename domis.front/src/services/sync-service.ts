import { fetchDataWithJsonBody } from "../helpers/fetch";
import { PUBLIC_API_URL } from "$env/static/public";

async function subscribeToNewsletter(email: string) {
  await fetchDataWithJsonBody(
    `${PUBLIC_API_URL}/api/sync/newsletter`,
    "POST",
    JSON.stringify(email)
  );
}

export const syncService = {
  subscribeToNewsletter,
};
