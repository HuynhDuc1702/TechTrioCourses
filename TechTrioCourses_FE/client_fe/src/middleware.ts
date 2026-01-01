import { NextResponse } from 'next/server';
import type { NextRequest } from 'next/server';

// ==================== ROUTE CONFIGURATION ====================

// Public routes - accessible to everyone (including guests)
const PUBLIC_ROUTES = [
  '/',
  '/auth/login',
  '/auth/register',
  '/auth/forgot-passwords',
  '/auth/reset-passwords',
  '/auth/verify-otp',
  '/courses',
];

// Routes that start with these paths are public
const PUBLIC_ROUTE_PREFIXES = [
  '/courses/', // Allow viewing individual courses
];

// Protected routes - require authentication
const PROTECTED_ROUTE_PREFIXES = [
  '/instructor',
  '/admin',
  '/shared',
  '/student',
];

// Role-based routes
const INSTRUCTOR_ROUTES = ['/instructor'];
const ADMIN_ROUTES = ['/admin'];

// ==================== HELPER FUNCTIONS ====================

function isPublicRoute(pathname: string): boolean {
  // Check exact matches
  if (PUBLIC_ROUTES.includes(pathname)) {
    return true;
  }
  
  // Check prefixes
  return PUBLIC_ROUTE_PREFIXES.some(prefix => pathname.startsWith(prefix));
}

function isProtectedRoute(pathname: string): boolean {
  return PROTECTED_ROUTE_PREFIXES.some(prefix => pathname.startsWith(prefix));
}

function getUserFromCookie(request: NextRequest) {
  try {
    const userCookie = request.cookies.get('user');
    if (!userCookie) {
      return null;
    }
    // Decode the URL-encoded cookie value
    const decodedValue = decodeURIComponent(userCookie.value);
    return JSON.parse(decodedValue);
  } catch (error) {
    console.error('Error parsing user cookie:', error);
    return null;
  }
}

function hasValidToken(request: NextRequest): boolean {
  const accessToken = request.cookies.get('accessToken');
  const localStorageToken = request.cookies.get('accessTokenFromStorage');
  return !!(accessToken || localStorageToken);
}

// ==================== MIDDLEWARE ====================

export function middleware(request: NextRequest) {
  const { pathname } = request.nextUrl;

  // Skip middleware for static files, API routes, and Next.js internals
  if (
    pathname.startsWith('/_next') ||
    pathname.startsWith('/api') ||
    pathname.includes('.') // Static files
  ) {
    return NextResponse.next();
  }

  // ✅ Public routes - allow access
  if (isPublicRoute(pathname)) {
    return NextResponse.next();
  }

  // ✅ Check if route requires authentication
  if (isProtectedRoute(pathname)) {
    const hasToken = hasValidToken(request);
    
    // No token - redirect to login
    if (!hasToken) {
      console.log('🔍 [Middleware] No valid token found for:', pathname);
      const loginUrl = new URL('/auth/login', request.url);
      loginUrl.searchParams.set('redirect', pathname);
      return NextResponse.redirect(loginUrl);
    }

    // Check role-based access
    const user = getUserFromCookie(request);
    console.log('🔍 [Middleware] User from cookie:', user ? `Role ${user.role}` : 'null');
    
    // Instructor routes - only for instructors and admins (role 3 and 1)
    if (pathname.startsWith('/instructor')) {
      if (user && user.role !== 3 && user.role !== 1) {
        return NextResponse.redirect(new URL('/courses', request.url));
      }
    }

    // Admin routes - only for admins (role 1)
    if (pathname.startsWith('/admin')) {
      if (user && user.role !== 1) {
        return NextResponse.redirect(new URL('/courses', request.url));
      }
    }

    return NextResponse.next();
  }

  // Default - allow access
  return NextResponse.next();
}

// ==================== CONFIG ====================

export const config = {
  matcher: [
    /*
     * Match all request paths except:
     * - _next/static (static files)
     * - _next/image (image optimization files)
     * - favicon.ico (favicon file)
     * - public files (public folder)
     */
    '/((?!_next/static|_next/image|favicon.ico|.*\\..*|public).*)',
  ],
};
