let headers = {};

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

  console.info(`Fetch fail: ${res.status}`);
  // throw new Error(`Fetch fail: ${res.status}`);
}
