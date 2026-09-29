/**
 * Thin fetch wrapper for the DevForge backend.
 * Assumes `/api/*` calls are proxied or same-origin. On a non-2xx response it
 * throws an Error whose message is taken from the backend's `{ message }` body
 * (falling back to the status text).
 */

async function request<T>(path: string, options: RequestInit = {}): Promise<T> {
  const headers: Record<string, string> = {
    'Content-Type': 'application/json',
    ...(options.headers as Record<string, string> | undefined)
  }

  const token = localStorage.getItem('devforge_token')
  if (token) {
    headers['Authorization'] = `Bearer ${token}`
  }

  const res = await fetch(`/api${path}`, { ...options, headers, credentials: 'same-origin' })

  if (!res.ok) {
    let message = res.statusText || 'Request failed'
    try {
      const body = await res.json()
      if (body?.message) message = body.message
    } catch {
      /* ignore non-JSON error bodies */
    }
    throw new Error(message)
  }

  if (res.status === 204) {
    return undefined as T
  }
  return res.json() as Promise<T>
}

export const http = {
  get: <T>(path: string) => request<T>(path),
  post: <T>(path: string, body: unknown) =>
    request<T>(path, { method: 'POST', body: JSON.stringify(body) })
}