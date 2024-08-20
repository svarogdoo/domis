let headers = {};

async function handleResponse(res: Response) {
  if (res.ok) {
    if (res.status === 204) return null;

    const isJson = res.headers
      .get("content-type")
      ?.includes("application/json");

    if (!isJson) return null;

    return res.json();
  }

  throw new Error(`Fetch fail: ${res.status}`);
}

export async function fetchData<T>(url: string, method?: string): Promise<T> {
  return fetch(url, {
    method,
    headers,
  }).then(handleResponse);
}
