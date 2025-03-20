import { defineConfig, loadEnv } from 'vite';
import react from '@vitejs/plugin-react';
import tailwindcss from '@tailwindcss/vite';

// https://vite.dev/config/
export default defineConfig(({ mode }) => {
  const env = loadEnv(mode, process.cwd(), '');

  return {
    server: {
      proxy: {
        '/api': {
          target: env.BACKEND_PATH,
          changeOrigin: true,
          secure: false,
        },
        '/api-events': {
          target: 'ws://localhost:5174',
          ws: true,
          rewriteWsOrigin: true,
        },
      },
    },
    plugins: [react(), tailwindcss()],
    resolve: {
      alias: {
        src: '/src',
      },
    },
  };
});
