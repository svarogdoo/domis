import { get } from "svelte/store";
import { API_URL } from "../config";
import { userStore } from "../stores/user";

let svelteKitFetch: typeof fetch = fetch; // resolves warning
let headers: { [key: string]: string } = {};

export function initFetch(fetch: typeof window.fetch) {
  svelteKitFetch = fetch;
}

export const setAuthToken = (token: string | null) => {
  if (token) {
    headers["Authorization"] = `Bearer ${token}`;
  } else {
    delete headers["Authorization"]; // Remove the header if no token is provided
  }
};

export async function fetchData<T>(url: string, method?: string): Promise<T> {
  return svelteKitFetch(url, {
    method,
    headers,
  }).then(handleResponse);
}

export async function putDataWithJsonBody(url: string, json: string) {
  return svelteKitFetch(url, {
    method: "put",
    body: json,
    headers: {
      "Content-Type": "application/json",
      ...headers,
    },
  }).then(handleResponse);
}

export async function deleteData(url: string) {
  return svelteKitFetch(url, {
    method: "delete",
    headers,
  }).then(handleResponse);
}

export async function fetchDataWithJsonBody<T>(
  url: string,
  method: string,
  json: string
): Promise<T> {
  return svelteKitFetch(url, {
    method,
    body: json,
    headers: {
      "Content-Type": "application/json",
      ...headers,
    },
  }).then(handleResponse);
}

async function handleResponse(res: Response) {
  if (res.ok) {
    if (res.status === 204) return null;

    const isJson = res.headers
      .get("content-type")
      ?.includes("application/json");

    if (!isJson) return null;

    return res.json();
  }

  handleTokenExpired(res.status);

  const error = new Error(res.statusText) as any;
  error.status = res.status;

  // Attempt to parse the response body for error details
  try {
    const errorBody = await res.json();
    error.errors = errorBody.errors;
    error.title = errorBody.title;
  } catch (e) {
    // console.error("Failed to parse error response:", e);
  }

  throw error; // Throw the error object with status and error details
}

async function handleTokenExpired(responseStatus: number) {
  var user = get(userStore);

  if (responseStatus === 401 && user.refreshToken) {
    const result = await svelteKitFetch(`${API_URL}/refresh`, {
      method: "POST",
      body: JSON.stringify({ refreshToken: user.refreshToken }),
      headers: {
        "Content-Type": "application/json",
        ...headers,
      },
    }).then(handleTokenExpiredResponse);

    if (result) {
      userStore.refreshUserSession(result);
    } else {
      userStore.logoutUser();
    }
  }
}

async function handleTokenExpiredResponse(res: Response) {
  if (res.ok) {
    if (res.status === 204) return null;

    const isJson = res.headers
      .get("content-type")
      ?.includes("application/json");

    if (!isJson) return null;

    return res.json();
  } else {
    return null;
  }
}
