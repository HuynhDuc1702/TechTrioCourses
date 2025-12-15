"use client";

import { useEffect } from 'react';
import { useRouter } from 'next/navigation';
import { useAuth } from '@/contexts/AuthContext';
import { UserRoleEnum } from '@/services/userAPI';

export default function InstructorPage() {
  const router = useRouter();
  const { user, loading } = useAuth();

  useEffect(() => {
    if (!loading) {
      if (!user || (user.role !== UserRoleEnum.Instructor && user.role !== UserRoleEnum.Admin)) {
        router.push('/');
      } else {
        router.push('/instructor/courses');
      }
    }
  }, [user, loading, router]);

  return (
    <div className="min-h-screen flex items-center justify-center">
      <div className="text-gray-600">Loading...</div>
    </div>
  );
}
