import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import { login as apiLogin, register as apiRegister, type LoginPayload, type RegisterPayload, type User } from '@/api/auth'

const TOKEN_KEY = 'devforge_token'
const USER_KEY = 'devforge_user'

function loadUser(): User | null {
  const raw = localStorage.getItem(USER_KEY)
  if (!raw) return null
  try {
    return JSON.parse(raw) as User
  } catch {
    return null
  }
}

export const useAuthStore = defineStore('auth', () => {
  const token = ref<string | null>(localStorage.getItem(TOKEN_KEY))
  const user = ref<User | null>(loadUser())
  const loading = ref(false)

  const isAuthenticated = computed(() => !!token.value && !!user.value)
  const displayName = computed(() => user.value?.fullName || user.value?.username || 'Guest')

  function setSession(nextToken: string, nextUser: User) {
    token.value = nextToken
    user.value = nextUser
    localStorage.setItem(TOKEN_KEY, nextToken)
    localStorage.setItem(USER_KEY, JSON.stringify(nextUser))
  }

  async function login(payload: LoginPayload) {
    loading.value = true
    try {
      const result = await apiLogin(payload)
      setSession(result.token, result.user)
      return result
    } finally {
      loading.value = false
    }
  }

  async function register(payload: RegisterPayload) {
    loading.value = true
    try {
      const result = await apiRegister(payload)
      setSession(result.token, result.user)
      return result
    } finally {
      loading.value = false
    }
  }

  async function logout() {
    token.value = null
    user.value = null
    localStorage.removeItem(TOKEN_KEY)
    localStorage.removeItem(USER_KEY)
  }

  return { token, user, loading, isAuthenticated, displayName, login, register, logout }
})