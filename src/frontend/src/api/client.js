import axios from 'axios';
import { authService } from 'src/shared/Auth/authService';
import { globalStore } from 'src/stores/globalStore';

let activeRequests = 0;

export const apiClient = axios.create({
  baseURL: '/api',
  timeout: 20000,
  headers: {
    'Content-Type': 'application/json',
  },
});

apiClient.defaults.silent = false;
apiClient.defaults.withHeaders = false;

// Перехватчик для запросов
apiClient.interceptors.request.use(config => {
  if (!config.silent) {
    activeRequests++;
    globalStore.loading.set(true);
  }
  const token = authService.restore();
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

// Для уменьшения счетчика и скрытия лоадера
const decrementRequests = () => {
  activeRequests = Math.max(0, activeRequests - 1);
  if (activeRequests === 0) {
    globalStore.loading.set(false);
  }
};

// Перехватчик для ответов
apiClient.interceptors.response.use(
  response => {
    const { withHeaders, silent } = response.config;
    !silent && decrementRequests();
    return withHeaders ? response : response.data;
  },
  error => {
    const { silent } = error.config;
    !silent && decrementRequests();
    const [message = error.message] = Object.values(
      error.response?.data?.errors || {}
    );
    globalStore.merge({ serverError: { message } });
    return Promise.reject(new Error(message));
  }
);
