"use client";

import React, { createContext, useState, useContext, useEffect } from 'react';
import { accountService, AuthResult, AccountResponse } from '@/services/accountAPI';
import { userAPI, UserResponse } from '@/services/userAPI';

interface User {
  accountId: string;
  userId: string;
  email: string;
  fullName: string;
  avatarUrl?: string;
  role: number;
}

interface AuthContextType {
  user: User | null;
  loading: boolean;
  login: (email: string, password: string) => Promise<void>;
  register: (email: string, password: string, fullName: string, avatarUrl?: string) => Promise<{ account: AccountResponse; message: string }>;
  logout: () => void;
  isAuthenticated: boolean;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const [user, setUser] = useState<User | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    // Check if user is already logged in
    const token = accountService.getAccessToken();
    if (token) {
      const storedUser = localStorage.getItem('user');
      if (storedUser) {
        try {
          setUser(JSON.parse(storedUser));
        } catch (error) {
          console.error('Error parsing stored user:', error);
          accountService.logout();
        }
      }
    }
    setLoading(false);
  }, []);

  const login = async (email: string, password: string) => {
    try {
      console.log('🔍 [DEBUG] Login attempt:', { email });
      const authResult: AuthResult = await accountService.login({ email, password });
      console.log('✅ [DEBUG] Login successful, tokens received');
      
      // Decode JWT to get accountId
      const tokenPayload = JSON.parse(atob(authResult.accessToken.split('.')[1]));
      const accountId = tokenPayload.sub || tokenPayload.accountId || tokenPayload.nameid;
      console.log('🔍 [DEBUG] Decoded accountId from token:', accountId);
      
      // After login, fetch user details
      const userInfo = await userAPI.getUserByAccountId(accountId);
      console.log('✅ [DEBUG] User info fetched:', userInfo);
      
      const userData: User = {
        accountId: userInfo.accountId,
        userId: userInfo.id,
        email: email,
        fullName: userInfo.fullName,
        avatarUrl: userInfo.avatarUrl,
        role: userInfo.role,
      };
      
      localStorage.setItem('user', JSON.stringify(userData));
      // Set cookie for middleware access
      document.cookie = `user=${JSON.stringify(userData)}; path=/; max-age=${60 * 60 * 24 * 7}`; // 7 days
      document.cookie = `accessTokenFromStorage=true; path=/; max-age=${60 * 60 * 24 * 7}`;
      
      setUser(userData);
      console.log('✅ [DEBUG] Login complete, user data saved');
    } catch (error: any) {
      console.error('❌ [DEBUG] Login error:', error);
      console.error('❌ [DEBUG] Error response:', error.response?.data);
      throw error;
    }
  };

  const register = async (email: string, password: string, fullName: string, avatarUrl?: string) => {
    const result = await accountService.register({ 
      email, 
      password, 
      fullName, 
      avatarUrl 
    });
    return result;
  };

  const logout = () => {
    accountService.logout();
    setUser(null);
  };

  return (
    <AuthContext.Provider value={{ 
      user, 
      loading,
      login, 
      register, 
      logout, 
      isAuthenticated: !!user 
    }}>
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error('useAuth must be used within AuthProvider');
  }
  return context;
};
