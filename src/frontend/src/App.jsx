import { QueryClientProvider } from '@tanstack/react-query'
import { App as AntApp, ConfigProvider } from 'antd'
import { RouterProvider, createBrowserRouter } from 'react-router'
import { routes } from './pages'
// import { queryClient } from '~/shared/queryClient'
import { lightTheme } from './styles/themes'
import { AuthProvider } from './shared/Auth/AuthProvider'

const router = createBrowserRouter(routes)

function App() {
  return (
    // <QueryClientProvider client={queryClient}>
      <ConfigProvider theme={lightTheme}>
      <AntApp
        message={{
          top: '90vh',
          duration: 2,
          maxCount: 5,
        }}
        notification={
          {
            placement: 'bottomRight',
            bottom: 50,
            duration: 5,
          }
        }
      >
          <AuthProvider>
            <RouterProvider router={router} />
          </AuthProvider>
        </AntApp>
      </ConfigProvider>
    // </QueryClientProvider>
  );
}

export default App;
