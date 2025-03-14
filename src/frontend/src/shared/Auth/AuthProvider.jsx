import { authService } from './authService'
import { AuthContext } from './AuthContext'
import { useState } from 'react'
// import { jwtDecode } from 'jwt-decode'
// import { UserRoleEnumStr } from '~/shared/auth/UserRoleEnum'

export function AuthProvider({ children }) {
  const [authToken, setAuthToken] = useState(authService.restore())

  return (
    <AuthContext.Provider
      value={{
        authorized: !!authToken,
        authInfo: authToken
          ? (() => {
              // const tokenPayload = jwtDecode(authToken)

              return {
                authToken,
                // user: {
                //   // isAdmin: UserRoleEnumStr.Admin === tokenPayload.role,
                //   // isStudent: UserRoleEnumStr.Student === tokenPayload.role,
                //   // isMentor: UserRoleEnumStr.Mentor === tokenPayload.role,
                //   userId: tokenPayload.userId,
                // },
              }
            })()
          : null,
        login(token) {
          authService.store(token)
          setAuthToken(token)
        },
        logout() {
          authService.remove()
          setAuthToken(null)
          history.pushState(null, '', '/login')
        },
      }}
    >
      {children}
    </AuthContext.Provider>
  )
}
