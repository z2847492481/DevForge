import { http } from './http'

export interface LoginPayload {
  usernameOrEmail: string
  password: string
}

export interface RegisterPayload {
  username: string
  email: string
  password: string
  fullName?: string
}

export interface User {
  id: number
  username: string
  email: string
  fullName?: string
  role: string
}

export interface AuthResponse {
  token: string
  expiresAt: number
  user: User
}

export function login(payload: LoginPayload): Promise<AuthResponse> {
  return http.post<AuthResponse>('/auth/login', payload)
}

export function register(payload: RegisterPayload): Promise<AuthResponse> {
  return http.post<AuthResponse>('/auth/register', payload)
}

export function logout(): Promise<void> {
  return Promise.resolve()
}