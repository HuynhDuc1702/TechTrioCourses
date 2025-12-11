"use client";

import React from "react";
import Link from "next/link";
import { useAuth } from "@/contexts/AuthContext";
import { useRouter } from "next/navigation";
import { UserRoleEnum } from "@/services/userAPI";

export default function Header() {
  const { user, logout, isAuthenticated } = useAuth();
  const router = useRouter();

  const handleLogout = () => {
    logout();
    router.push("/auth/login");
  };

  const getRoleBasedLinks = () => {
    if (!user) return null;

    const links = [
      { href: "/", label: "Home", show: true }
    ];

    // Student: only show Student link
    if (user.role === UserRoleEnum.Student) {
      links.push({ href: "/student", label: "Student", show: true });
    }
    
    // Instructor: show Student and Instructor links
    if (user.role === UserRoleEnum.Instructor) {
      links.push({ href: "/student", label: "Student", show: true });
      links.push({ href: "/instructor", label: "Instructor", show: true });
    }
    
    // Admin: show all links
    if (user.role === UserRoleEnum.Admin) {
      links.push({ href: "/student", label: "Student", show: true });
      links.push({ href: "/instructor", label: "Instructor", show: true });
      links.push({ href: "/admin", label: "Admin", show: true });
    }

    return links.filter(link => link.show);
  };

  const roleLinks = getRoleBasedLinks();

  return (
    <header className="bg-white shadow-sm border-b border-gray-200">
      <div className="container mx-auto px-4 py-4 flex justify-between items-center">
        <Link href="/" className="text-xl font-semibold text-gray-800 hover:text-indigo-600 transition-colors">
          TechTrio Courses
        </Link>

        <nav className="flex items-center gap-6">
          

          {/* Role-based navigation links */}
          {roleLinks && roleLinks.map((link) => (
            <Link 
              key={link.href}
              href={link.href} 
              className="text-gray-700 hover:text-indigo-600 transition-colors font-medium"
            >
              {link.label}
            </Link>
          ))}
          
          <div className="flex items-center gap-4">
            {isAuthenticated && user ? (
              <>
                <div className="flex items-center gap-3">
                  <div className="w-10 h-10 bg-gradient-to-r from-blue-500 to-indigo-600 rounded-full flex items-center justify-center text-white font-semibold">
                    {user.fullName.charAt(0).toUpperCase()}
                  </div>
                  <span className="text-gray-700 font-medium">{user.fullName}</span>
                </div>
                <button
                  onClick={handleLogout}
                  className="px-4 py-2 text-sm font-medium text-gray-700 hover:text-gray-900 border border-gray-300 rounded-lg hover:bg-gray-50 transition-all"
                >
                  Logout
                </button>
              </>
            ) : (
              <>
                <Link
                  href="/auth/login"
                  className="px-4 py-2 text-sm font-medium text-gray-700 hover:text-gray-900 border border-gray-300 rounded-lg hover:bg-gray-50 transition-all"
                >
                  Login
                </Link>
                <Link
                  href="/auth/register"
                  className="px-4 py-2 text-sm font-medium text-white bg-gradient-to-r from-blue-500 to-indigo-600 rounded-lg hover:from-blue-600 hover:to-indigo-700 transition-all"
                >
                  Register
                </Link>
              </>
            )}
          </div>
        </nav>
      </div>
    </header>
  );
}
