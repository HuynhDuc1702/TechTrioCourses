"use client";

import React, { createContext, useState, useContext, useEffect } from 'react';
import { accountService, AuthResult, AccountResponse } from '@/services/accountAPI';
import { userAPI, UserResponse } from '@/services/userAPI';
import Cookies from 'js-cookie';

interface User {
  accountId: string;
  userId: string;
  email: string;
  fullName: string;
  avatarUrl?: string;
  role: number;
  createdAt: string;
}

interface AuthContextType {
  user: User | null;
  loading: boolean;
  login: (email: string, password: string) => Promise<void>;
  register: (email: string, password: string, fullName: string, avatarUrl?: string) => Promise<{ account: AccountResponse; message: string }>;
  logout: () => void;
  isAuthenticated: boolean;
  refreshUser: () => Promise<void>;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const [user, setUser] = useState<User | null>(null);
  const [loading, setLoading] = useState(true);

  // Helper to sync user state across storage and cookies
  const syncUserSession = (userData: User, refreshTokenExpiresAt?: string) => {
    // 1. Update State
    setUser(userData);

    // 2. Update LocalStorage
    localStorage.setItem('user', JSON.stringify(userData));


    // 3. Update Cookies
    // Try to get existing expiration from stored refresh token expiry, or default to 7 days

    const refreshTokenExpiresAtStr = Cookies.get('refreshTokenExpiresAt');
    let expiresOpt: Date | number = 7; // Default fallback

    if (refreshTokenExpiresAtStr) {
      expiresOpt = new Date(refreshTokenExpiresAtStr);
    }

    Cookies.set('user', JSON.stringify(userData), { path: '/', expires: expiresOpt, sameSite: 'Lax' });
  };

  useEffect(() => {
    const isAuthenticated = accountService.isAuthenticated();

    if (!isAuthenticated) {
      accountService.logout();
      setLoading(false);
      return;
    }

    const storedUser = localStorage.getItem("user");
    if (!storedUser) {
      accountService.logout();
      setLoading(false);
      return;
    }

    try {
      setUser(JSON.parse(storedUser));
    } catch {
      accountService.logout();
    }

    setLoading(false);
  }, []);

  const refreshUser = async () => {
    try {
      if (!user) return; 
      const userInfo = await userAPI.getUserByAccountId(user.accountId);

      const userData: User = {
        accountId: userInfo.accountId,
        userId: userInfo.id,
        email: user.email, 
        fullName: userInfo.fullName,
        avatarUrl: userInfo.avatarUrl,
        role: userInfo.role,
        createdAt: userInfo.createdAt,
      };

     
      syncUserSession(userData);

    } catch (error) {
      console.error('Failed to refresh user profile:', error);
    }
  };


  const login = async (email: string, password: string) => {
    try {
      console.log('🔍 [DEBUG] Login attempt:', { email });
      const authResult: AuthResult = await accountService.login({ email, password });
      console.log('✅ [DEBUG] Login successful, tokens received');

      // Decode JWT to get accountId
      const tokenPayload = JSON.parse(atob(authResult.accessToken.split('.')[1]));
      // Try multiple claim names (sub, accountId, nameid, or the full URI for NameIdentifier)
      const accountId = tokenPayload.sub ||
        tokenPayload.accountId ||
        tokenPayload.nameid ||
        tokenPayload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'];
      console.log('🔍 [DEBUG] Token payload:', tokenPayload);
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
        createdAt: userInfo.createdAt,
      };

      console.log('🔍 [DEBUG] Setting user data:', userData);

      syncUserSession(userData, authResult.refreshTokenExpiresAt);

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
      isAuthenticated: !!user,
      refreshUser
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
