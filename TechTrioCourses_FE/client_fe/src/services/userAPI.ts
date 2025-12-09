import axios from 'axios';
import { API_URLS, API_ENDPOINTS } from '@/constants/apiURL';

const API_BASE_URL = API_URLS.USER;

// ==================== INTERFACES ====================

export enum UserRoleEnum {
  Student = 0,
  Instructor = 1,
  Admin = 2
}

export interface UserResponse {
  id: string;
  accountId: string;
  fullName: string;
  avatarUrl?: string;
  role: UserRoleEnum;
  createdAt: string;
}

export interface CreateUserRequest {
  accountId: string;
  fullName: string;
  avatarUrl?: string;
  role?: UserRoleEnum;
}

export interface UpdateUserRequest {
  fullName?: string;
  avatarUrl?: string;
  role?: UserRoleEnum;
}

// ==================== AXIOS INSTANCE ====================

const axiosInstance = axios.create({
  baseURL: API_BASE_URL,
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

// Response interceptor
axiosInstance.interceptors.response.use(
  (response: any) => response,
  async (error: any) => {
    if (error.response?.status === 401) {
      localStorage.removeItem('accessToken');
      localStorage.removeItem('refreshToken');
      localStorage.removeItem('user');
      window.location.href = '/login';
    }
    return Promise.reject(error);
  }
);

// ==================== USER SERVICE ====================

export const userService = {
  /**
   * Get all users
   * GET: /api/Users
   */
  getAllUsers: async (): Promise<UserResponse[]> => {
    const response = await axiosInstance.get<UserResponse[]>(API_ENDPOINTS.USERS.BASE);
    return response.data;
  },

  /**
   * Get user by ID
   * GET: /api/Users/{id}
   */
  getUserById: async (id: string): Promise<UserResponse> => {
    const response = await axiosInstance.get<UserResponse>(
      `${API_ENDPOINTS.USERS.BASE}/${id}`
    );
    return response.data;
  },

  /**
   * Get user by account ID
   * GET: /api/Users/by-account/{accountId}
   */
  getUserByAccountId: async (accountId: string): Promise<UserResponse> => {
    const response = await axiosInstance.get<UserResponse>(
      `${API_ENDPOINTS.USERS.BASE}/by-account/${accountId}`
    );
    return response.data;
  },

  /**
   * Get users by multiple IDs
   * POST: /api/Users/get-by-ids
   */
  getUsersByIds: async (ids: string[]): Promise<UserResponse[]> => {
    const response = await axiosInstance.post<UserResponse[]>(
      `${API_ENDPOINTS.USERS.BASE}/get-by-ids`,
      ids
    );
    return response.data;
  },

  /**
   * Create new user
   * POST: /api/Users
   */
  createUser: async (data: CreateUserRequest): Promise<UserResponse> => {
    const response = await axiosInstance.post<UserResponse>(
      API_ENDPOINTS.USERS.BASE,
      data
    );
    return response.data;
  },

  /**
   * Update user by ID
   * PUT: /api/Users/{id}
   */
  updateUser: async (id: string, data: UpdateUserRequest): Promise<UserResponse> => {
    const response = await axiosInstance.put<UserResponse>(
      `${API_ENDPOINTS.USERS.BASE}/${id}`,
      data
    );
    return response.data;
  },

  /**
   * Get current user profile (helper method)
   */
  getCurrentUserProfile: async (): Promise<UserResponse | null> => {
    try {
      const user = localStorage.getItem('user');
      if (user) {
        const parsedUser = JSON.parse(user);
        return await userService.getUserById(parsedUser.userId);
      }
      return null;
    } catch (error) {
      console.error('Error getting current user profile:', error);
      return null;
    }
  },

  /**
   * Update current user profile (helper method)
   */
  updateCurrentUserProfile: async (data: UpdateUserRequest): Promise<UserResponse | null> => {
    try {
      const user = localStorage.getItem('user');
      if (user) {
        const parsedUser = JSON.parse(user);
        const updatedUser = await userService.updateUser(parsedUser.userId, data);
        
        // Update local storage
        localStorage.setItem('user', JSON.stringify({
          ...parsedUser,
          fullName: updatedUser.fullName,
          avatarUrl: updatedUser.avatarUrl,
          role: updatedUser.role
        }));
        
        return updatedUser;
      }
      return null;
    } catch (error) {
      console.error('Error updating current user profile:', error);
      return null;
    }
  },

  /**
   * Check if current user has specific role
   */
  hasRole: (role: UserRoleEnum): boolean => {
    try {
      const user = localStorage.getItem('user');
      if (user) {
        const parsedUser = JSON.parse(user);
        return parsedUser.role === role;
      }
      return false;
    } catch (error) {
      return false;
    }
  },

  /**
   * Check if current user is admin
   */
  isAdmin: (): boolean => {
    return userService.hasRole(UserRoleEnum.Admin);
  },

  /**
   * Check if current user is instructor
   */
  isInstructor: (): boolean => {
    return userService.hasRole(UserRoleEnum.Instructor);
  },

  /**
   * Check if current user is student
   */
  isStudent: (): boolean => {
    return userService.hasRole(UserRoleEnum.Student);
  },
};

export default userService;
