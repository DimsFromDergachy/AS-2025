import { authToken } from './authToken'
import { useState, createContext } from 'react'
import { jwtDecode } from 'jwt-decode'
// import { UserRoleEnumStr } from '~/shared/auth/UserRoleEnum'

const AuthContext = createContext({})

export function AuthProvider({ children }) {
  const [authTokenValue, setAuthTokenValue] = useState(authToken.restore())

  return (
    <AuthContext.Provider
      value={{
        authorized: !!authTokenValue,
        authInfo: authTokenValue
          ? (() => {
              const tokenPayload = jwtDecode(authTokenValue)

              return {
                tokenPayload,
                user: {
                  // isAdmin: UserRoleEnumStr.Admin === tokenPayload.role,
                  // isStudent: UserRoleEnumStr.Student === tokenPayload.role,
                  // isMentor: UserRoleEnumStr.Mentor === tokenPayload.role,
                  userId: tokenPayload.userId,
                },
              }
            })()
          : null,
        login(token) {
          authToken.store(token)
          setAuthTokenValue(token)
        },
        logout() {
          authToken.remove()
          setAuthTokenValue(null)
          history.pushState(null, '', '/login')
        },
      }}
    >
      {children}
    </AuthContext.Provider>
  )
}
