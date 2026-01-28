import axios, { AxiosInstance, InternalAxiosRequestConfig, AxiosError } from 'axios';
import { API_URLS, API_ENDPOINTS } from '@/constants/apiURL';
import Cookies from 'js-cookie';


export class TokenManager {
  private static readonly ACCESS_TOKEN_KEY = 'accessToken';
  private static readonly REFRESH_TOKEN_KEY = 'refreshToken';
  private static readonly ACCESS_EXPIRY_KEY = 'accessTokenExpiresAt';
  private static readonly REFRESH_EXPIRY_KEY = 'refreshTokenExpiresAt';
  private static readonly USER_KEY = 'user';


  static getAccessToken(): string | null {
    return Cookies.get(this.ACCESS_TOKEN_KEY) || null;
  }

  static getRefreshToken(): string | null {
    return Cookies.get(this.REFRESH_TOKEN_KEY) || null;
  }


  static setAccessToken(token: string): void {
    Cookies.set(this.ACCESS_TOKEN_KEY, token, { secure: true, sameSite: 'Lax' });
  }


  static setRefreshToken(token: string): void {
    Cookies.set(this.REFRESH_TOKEN_KEY, token, { secure: true, sameSite: 'Lax' });
  }


  static setTokens(accessToken: string, refreshToken: string, accessExpiresAt: string, refreshExpiresAt: string): void {

    const accessDate = new Date(accessExpiresAt);
    const refreshDate = new Date(refreshExpiresAt);

    Cookies.set(this.ACCESS_TOKEN_KEY, accessToken, { expires: refreshDate, secure: true, sameSite: 'Lax' });
    Cookies.set(this.REFRESH_TOKEN_KEY, refreshToken, { expires: refreshDate, secure: true, sameSite: 'Lax' });

    Cookies.set(this.ACCESS_EXPIRY_KEY, accessExpiresAt, { expires: refreshDate, secure: true, sameSite: 'Lax' });
    Cookies.set(this.REFRESH_EXPIRY_KEY, refreshExpiresAt, { expires: refreshDate, secure: true, sameSite: 'Lax' });
  }


  static clearTokens(): void {
    Cookies.remove(this.ACCESS_TOKEN_KEY);
    Cookies.remove(this.REFRESH_TOKEN_KEY);
    Cookies.remove(this.ACCESS_EXPIRY_KEY);
    Cookies.remove(this.REFRESH_EXPIRY_KEY);
    Cookies.remove(this.USER_KEY);

    Cookies.remove(this.USER_KEY);

    localStorage.removeItem(this.USER_KEY);
  }

  static isAccessTokenExpired(): boolean {
    const expiresAt = Cookies.get(this.ACCESS_EXPIRY_KEY);
    if (!expiresAt) return true;
    return new Date(expiresAt) <= new Date();
  }

  static isRefreshTokenExpired(): boolean {
    const expiresAt = Cookies.get(this.REFRESH_EXPIRY_KEY);
    if (!expiresAt) return true;
    return new Date(expiresAt) <= new Date();
  }


  static isAuthenticated(): boolean {
    return !!this.getRefreshToken() && !this.isRefreshTokenExpired();
  }
}


interface RefreshTokenResponse {
  accessToken: string;
  refreshToken: string;
  accessTokenExpiresAt: string;
  refreshTokenExpiresAt: string;
}

let isRefreshing = false;
let failedQueue: Array<{
  resolve: (value?: any) => void;
  reject: (reason?: any) => void;
}> = [];

const processQueue = (error: any = null, token: string | null = null): void => {
  failedQueue.forEach((prom) => {
    if (error) {
      prom.reject(error);
    } else {
      prom.resolve(token);
    }
  });

  failedQueue = [];
};


const refreshAccessToken = async (): Promise<string> => {
  const refreshToken = TokenManager.getRefreshToken();

  if (!refreshToken || TokenManager.isRefreshTokenExpired()) {
    throw new Error('No valid refresh token available');
  }

  try {
    const response = await axios.post<RefreshTokenResponse>(
      `${API_URLS.ACCOUNT}${API_ENDPOINTS.ACCOUNTS.REFRESH_TOKEN}`,
      { refreshToken },
      { withCredentials: true }
    );

    const { accessToken, refreshToken: newRefreshToken, accessTokenExpiresAt, refreshTokenExpiresAt } = response.data;
    TokenManager.setTokens(accessToken, newRefreshToken, accessTokenExpiresAt, refreshTokenExpiresAt);

    return accessToken;
  } catch (error) {
    TokenManager.clearTokens();
    throw error;
  }
};

const requestInterceptor = (config: InternalAxiosRequestConfig): InternalAxiosRequestConfig => {
  const token = TokenManager.getAccessToken();

  if (token && config.headers) {
    config.headers.Authorization = `Bearer ${token}`;
  }

  return config;
};


const requestErrorInterceptor = (error: any): Promise<never> => {
  return Promise.reject(error);
};


const responseInterceptor = (response: any) => {
  return response;
};


const responseErrorInterceptor = async (error: AxiosError): Promise<any> => {
  const originalRequest = error.config as InternalAxiosRequestConfig & { _retry?: boolean };


  const authEndpoints = [
    API_ENDPOINTS.ACCOUNTS.LOGIN,
    API_ENDPOINTS.ACCOUNTS.REGISTER,
    API_ENDPOINTS.ACCOUNTS.SEND_OTP,
    API_ENDPOINTS.ACCOUNTS.VERIFY_OTP,
    API_ENDPOINTS.ACCOUNTS.RESET_PASSWORD,
    API_ENDPOINTS.ACCOUNTS.CHANGE_PASSWORD,
  ];


  const isAuthEndpoint = authEndpoints.some(endpoint =>
    originalRequest.url?.includes(endpoint)
  );

  if (isAuthEndpoint) {
    return Promise.reject(error);
  }


  if (error.response?.status === 401 && !originalRequest._retry) {
    if (isRefreshing) {

      return new Promise((resolve, reject) => {
        failedQueue.push({ resolve, reject });
      })
        .then((token) => {
          if (originalRequest.headers) {
            originalRequest.headers.Authorization = `Bearer ${token}`;
          }
          return axios(originalRequest);
        })
        .catch((err) => {
          return Promise.reject(err);
        });
    }

    originalRequest._retry = true;
    isRefreshing = true;

    try {
      const newAccessToken = await refreshAccessToken();
      processQueue(null, newAccessToken);

      if (originalRequest.headers) {
        originalRequest.headers.Authorization = `Bearer ${newAccessToken}`;
      }

      return axios(originalRequest);
    } catch (refreshError) {
      processQueue(refreshError, null);

      if (typeof window !== 'undefined') {
        window.location.href = '/auth/login';
      }

      return Promise.reject(refreshError);
    } finally {
      isRefreshing = false;
    }
  }

  return Promise.reject(error);
};


export const createAxiosInstance = (baseURL: string): AxiosInstance => {
  const instance = axios.create({
    baseURL,
    withCredentials: true,
  });


  instance.interceptors.request.use(
    requestInterceptor,
    requestErrorInterceptor
  );


  instance.interceptors.response.use(
    responseInterceptor,
    responseErrorInterceptor
  );

  return instance;
};


export const accountAxios = createAxiosInstance(API_URLS.ACCOUNT);


export const userAxios = createAxiosInstance(API_URLS.USER);


export const courseAxios = createAxiosInstance(API_URLS.COURSE);

export const categoryAxios = createAxiosInstance(API_URLS.CATEGORY);

export const lessonAxios = createAxiosInstance(API_URLS.LESSON);

export const quizAxios = createAxiosInstance(API_URLS.QUIZ);
