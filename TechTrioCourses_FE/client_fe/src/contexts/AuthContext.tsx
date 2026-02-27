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


  const syncUserSession = (userData: User, refreshTokenExpiresAt?: string) => {

    setUser(userData);
    localStorage.setItem('user', JSON.stringify(userData));

    const refreshTokenExpiresAtStr = Cookies.get('refreshTokenExpiresAt');
    let expiresOpt: Date | number = 7;

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

      const authResult: AuthResult = await accountService.login({ email, password });


      const tokenPayload = JSON.parse(atob(authResult.accessToken.split('.')[1]));
      // .NET may emit the account ID under different claim names depending on configuration.
      // Priority: sub (JWT standard) → nameid (short form) → long URI form
      const accountId =
        tokenPayload.sub ||
        tokenPayload.accountId ||
        tokenPayload.nameid ||
        tokenPayload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'];

      if (!accountId) throw new Error('Invalid token: account ID claim not found');




      const userInfo = await userAPI.getUserByAccountId(accountId);


      const userData: User = {
        accountId: userInfo.accountId,
        userId: userInfo.id,
        email: email,
        fullName: userInfo.fullName,
        avatarUrl: userInfo.avatarUrl,
        role: userInfo.role,
        createdAt: userInfo.createdAt,
      };



      syncUserSession(userData, authResult.refreshTokenExpiresAt);


    } catch (error: any) {
      console.error('Login error:', error);
      console.error('Error response:', error.response?.data);
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
