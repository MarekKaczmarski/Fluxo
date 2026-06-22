const API_BASE_URL = (import.meta.env.VITE_API_URL ?? 'http://localhost:7001/api').replace(
  /\/$/,
  '',
)

interface ExpectedErrorData {
  title?: string
  errors?: Record<string, string[]>
}

export class ApiError extends Error {
  status: number
  data: unknown

  constructor(status: number, data: unknown) {
    const errorData = data as ExpectedErrorData

    super(errorData?.title || `Request failed with status ${status}`)
    this.name = 'ApiError'
    this.status = status
    this.data = data
  }
}

export async function apiRequest<T>(path: string, init?: RequestInit): Promise<T> {
  const response = await fetch(`${API_BASE_URL}${path}`, {
    ...init,
    headers: {
      'Content-Type': 'application/json',
      ...init?.headers,
    },
  })

  if (!response.ok) {
    let body: unknown = null
    try {
      body = await response.json()
    } catch {
      try {
        body = { title: await response.text() }
      } catch {
        body = { title: `Request failed with status ${response.status}` }
      }
    }
    throw new ApiError(response.status, body)
  }

  if (response.status === 204) {
    return undefined as T
  }

  return response.json() as Promise<T>
}
