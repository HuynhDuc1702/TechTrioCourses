import axios from 'axios';
import { API_URLS, API_ENDPOINTS } from '@/constants/apiURL';

const API_BASE_URL = API_URLS.ACCOUNT;

// ==================== INTERFACES ====================

export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest {
  email: string;
  password: string;
  fullName: string;
  avatarUrl?: string;
}

export interface AuthResult {
  accessToken: string;
  refreshToken: string;
  accessTokenExpiresAt: string;
  refreshTokenExpiresAt: string;
}

export interface AccountResponse {
  accountId: string;
  userId: string;
  email: string;
  fullName: string;
  avatarUrl?: string;
  role: number;
  createdAt: string;
}

export interface RefreshTokenRequest {
  refreshToken: string;
}

export interface SendOtpRequest {
  email: string;
  purpose: string; // "Registration", "PasswordReset", "EmailVerification"
}

export interface VerifyOtpRequest {
  email: string;
  code: string;
}

export interface ChangePasswordRequest {
  email: string;
  oldPassword: string;
  newPassword: string;
}

export interface OtpResponse {
  message: string;
  expiresAt: string;
}

// ==================== AXIOS INSTANCE ====================

const axiosInstance = axios.create({
  baseURL: API_BASE_URL,
  withCredentials: true, // Important for cookies
});

// Request interceptor
axiosInstance.interceptors.request.use(
  (config: any) => {
    const token = localStorage.getItem('accessToken');
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error: any) => Promise.reject(error)
);

// Response interceptor with auto token refresh
axiosInstance.interceptors.response.use(
  (response: any) => response,
  async (error: any) => {
    const originalRequest = error.config;

    if (error.response?.status === 401 && !originalRequest._retry) {
      originalRequest._retry = true;

      try {
        const refreshToken = localStorage.getItem('refreshToken');
        if (refreshToken) {
          const response = await accountService.refreshToken({ refreshToken });
          localStorage.setItem('accessToken', response.accessToken);
          localStorage.setItem('refreshToken', response.refreshToken);
          
          originalRequest.headers.Authorization = `Bearer ${response.accessToken}`;
          return axiosInstance(originalRequest);
        }
      } catch (refreshError) {
        accountService.logout();
        window.location.href = '/login';
        return Promise.reject(refreshError);
      }
    }

    return Promise.reject(error);
  }
);

// ==================== ACCOUNT SERVICE ====================

export const accountService = {
  /**
   * Login user
   * POST: /api/Accounts/login
   */
  login: async (data: LoginRequest): Promise<AuthResult> => {
    const response = await axiosInstance.post<AuthResult>(
      API_ENDPOINTS.ACCOUNTS.LOGIN,
      data
    );
    
    if (response.data.accessToken) {
      localStorage.setItem('accessToken', response.data.accessToken);
      localStorage.setItem('refreshToken', response.data.refreshToken);
    }
    
    return response.data;
  },

  /**
   * Register new user (returns account info, requires OTP verification)
   * POST: /api/Accounts/register
   */
  register: async (data: RegisterRequest): Promise<{ account: AccountResponse; message: string }> => {
    const response = await axiosInstance.post<{ account: AccountResponse; message: string }>(
      API_ENDPOINTS.ACCOUNTS.REGISTER,
      data
    );
    return response.data;
  },

  /**
   * Refresh access token
   * POST: /api/Accounts/refresh-token
   */
  refreshToken: async (data: RefreshTokenRequest): Promise<AuthResult> => {
    const response = await axiosInstance.post<AuthResult>(
      API_ENDPOINTS.ACCOUNTS.REFRESH_TOKEN,
      data
    );
    
    if (response.data.accessToken) {
      localStorage.setItem('accessToken', response.data.accessToken);
      localStorage.setItem('refreshToken', response.data.refreshToken);
    }
    
    return response.data;
  },

  /**
   * Send OTP to email
   * POST: /api/Accounts/send-otp
   */
  sendOtp: async (data: SendOtpRequest): Promise<OtpResponse> => {
    const response = await axiosInstance.post<OtpResponse>(
      '/Accounts/send-otp',
      data
    );
    return response.data;
  },

  /**
   * Verify OTP and activate account
   * POST: /api/Accounts/verify-otp
   */
  verifyOtp: async (data: VerifyOtpRequest): Promise<{ message: string }> => {
    const response = await axiosInstance.post<{ message: string }>(
      '/Accounts/verify-otp',
      data
    );
    return response.data;
  },

  /**
   * Change password
   * POST: /api/Accounts/change-password
   */
  changePassword: async (data: ChangePasswordRequest): Promise<{ message: string }> => {
    const response = await axiosInstance.post<{ message: string }>(
      '/Accounts/change-password',
      data
    );
    return response.data;
  },

  /**
   * Logout user
   */
  logout: () => {
    localStorage.removeItem('accessToken');
    localStorage.removeItem('refreshToken');
    localStorage.removeItem('user');
  },

  /**
   * Get access token
   */
  getAccessToken: () => {
    return localStorage.getItem('accessToken');
  },

  /**
   * Get refresh token
   */
  getRefreshToken: () => {
    return localStorage.getItem('refreshToken');
  },

  /**
   * Check if user is authenticated
   */
  isAuthenticated: () => {
    return !!localStorage.getItem('accessToken');
  },
};

export default accountService;