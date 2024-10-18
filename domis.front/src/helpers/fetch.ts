let headers: { [key: string]: string } = {};

export const setAuthToken = (token: string | null) => {
  if (token) {
    headers["Authorization"] = `Bearer ${token}`;
  } else {
    delete headers["Authorization"]; // Remove the header if no token is provided
  }
};

export async function fetchData<T>(url: string, method?: string): Promise<T> {
  return fetch(url, {
    method,
    headers,
  }).then(handleResponse);
}

export async function putDataWithJsonBody(url: string, json: string) {
  return fetch(url, {
    method: "put",
    body: json,
    headers: {
      "Content-Type": "application/json",
      ...headers,
    },
  }).then(handleResponse);
}

export async function deleteData(url: string) {
  return fetch(url, {
    method: "delete",
    headers,
  }).then(handleResponse);
}

export async function fetchDataWithJsonBody<T>(
  url: string,
  method: string,
  json: string
): Promise<T> {
  return fetch(url, {
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

  const error = new Error(res.statusText) as any;
  error.status = res.status;

  // Attempt to parse the response body for error details
  try {
    const errorBody = await res.json(); // Parse the error body as JSON
    error.errors = errorBody.errors; // Attach the errors to the error object
    error.title = errorBody.title; // Optionally attach other relevant properties
  } catch (e) {
    // console.error("Failed to parse error response:", e);
  }

  throw error; // Throw the error object with status and error details
}
