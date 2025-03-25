import React, { useEffect, useState } from 'react';
// import { QueryClientProvider } from '@tanstack/react-query';
import { App as AntApp, ConfigProvider } from 'antd';
import { ruRu } from 'antd/lib/locale';
import { RouterProvider, createBrowserRouter } from 'react-router';
import { getRoutes } from './pages/routes';
import { apiClient } from './api/client';
// import { queryClient } from '~/shared/queryClient'
import { lightTheme } from './styles/themes';
import { AuthProvider } from './shared/Auth/AuthProvider';
import { globalStore } from './stores/globalStore';

function App() {
  const [router, setRouter] = useState(null);

  useEffect(() => {
    apiClient.get('/menu/list').then(({ items }) => {
      globalStore.menuItems.set(items);
      const modelPages = items.reduce(
        (acc, el) => (el.modelKey ? [...acc, el.modelKey] : acc),
        []
      );
      const routes = getRoutes(modelPages);
      setRouter(createBrowserRouter(routes));
    });
  }, []);

  return (
    // <QueryClientProvider client={queryClient}>
    <ConfigProvider locale={ruRu} theme={lightTheme}>
      <AntApp
        message={{
          top: '90vh',
          duration: 2,
          maxCount: 5,
        }}
        notification={{
          placement: 'bottomRight',
          bottom: 50,
          duration: 5,
        }}
      >
        <AuthProvider>
          {router && <RouterProvider router={router} />}
        </AuthProvider>
      </AntApp>
    </ConfigProvider>
    // </QueryClientProvider>
  );
}

export default App;
