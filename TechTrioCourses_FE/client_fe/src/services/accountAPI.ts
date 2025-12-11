import { API_ENDPOINTS } from '@/constants/apiURL';
import { accountAxios, TokenManager } from '@/middleware/axiosMiddleware';

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

// ==================== ACCOUNT SERVICE ====================

export const accountService = {

  
  /**
   * Login user
   * POST: /api/Accounts/login
   */
  login: async (data: LoginRequest): Promise<AuthResult> => {
    const response = await accountAxios.post<AuthResult>(
      API_ENDPOINTS.ACCOUNTS.LOGIN,
      data
    );
    
    if (response.data.accessToken) {
      TokenManager.setTokens(response.data.accessToken, response.data.refreshToken);
    }
    
    return response.data;
  },

  /**
   * Register new user (returns account info, requires OTP verification)
   * POST: /api/Accounts/register
   */
  register: async (data: RegisterRequest): Promise<{ account: AccountResponse; message: string }> => {
    const response = await accountAxios.post<{ account: AccountResponse; message: string }>(
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
    const response = await accountAxios.post<AuthResult>(
      API_ENDPOINTS.ACCOUNTS.REFRESH_TOKEN,
      data
    );
    
    if (response.data.accessToken) {
      TokenManager.setTokens(response.data.accessToken, response.data.refreshToken);
    }
    
    return response.data;
  },

  /**
   * Send OTP to email
   * POST: /api/Accounts/send-otp
   */
  sendOtp: async (data: SendOtpRequest): Promise<OtpResponse> => {
    const response = await accountAxios.post<OtpResponse>(
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
    const response = await accountAxios.post<{ message: string }>(
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
    const response = await accountAxios.post<{ message: string }>(
      '/Accounts/change-password',
      data
    );
    return response.data;
  },

  /**
   * Logout user
   */
  logout: () => {
    TokenManager.clearTokens();
  },

  /**
   * Get access token
   */
  getAccessToken: () => {
    return TokenManager.getAccessToken();
  },

  /**
   * Get refresh token
   */
  getRefreshToken: () => {
    return TokenManager.getRefreshToken();
  },

  /**
   * Check if user is authenticated
   */
  isAuthenticated: () => {
    return TokenManager.isAuthenticated();
  },
};

export default accountService;