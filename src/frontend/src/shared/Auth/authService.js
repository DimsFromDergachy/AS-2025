const AUTH_TOKEN_KEY = 'authToken'

export const authService = {
  store(token) {
    localStorage.setItem(AUTH_TOKEN_KEY, token)
  },
  restore() {
    return localStorage.getItem(AUTH_TOKEN_KEY)
  },
  remove() {
    localStorage.removeItem(AUTH_TOKEN_KEY)
  },
}
