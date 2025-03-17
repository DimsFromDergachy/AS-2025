import axios from 'axios';
import { authService } from 'src/shared/Auth/authService';
import { globalStore } from 'src/stores/globalStore';

export const apiClient = axios.create({
  baseURL: '/api',
  timeout: 10000,
  headers: {
    'Content-Type': 'application/json',
  },
});

// Перехватчик для запросов
apiClient.interceptors.request.use(config => {
  globalStore.loading.set(true);
  const token = authService.restore();
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

// Перехватчик для ошибок
apiClient.interceptors.response.use(
  response => {
    globalStore.loading.set(false);
    return response.data;
  },
  error => {
    const [message = error.message] = Object.values(
      error.response?.data?.errors || {}
    );
    globalStore.merge({ loading: false, serverError: { message } });
    return Promise.reject(new Error(message));
  }
);
