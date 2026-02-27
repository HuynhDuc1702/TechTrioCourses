import { NextResponse } from 'next/server';
import type { NextRequest } from 'next/server';


const PUBLIC_ROUTES = [
  '/',
  '/auth/login',
  '/auth/register',
  '/auth/forgot-passwords',
  '/auth/reset-passwords',
  '/auth/verify-otp',
  '/courses',
];


const PUBLIC_ROUTE_PREFIXES = [
  '/courses/',
];

const PROTECTED_ROUTE_PREFIXES = [
  '/instructor',
  '/admin',
  '/shared',
  '/student',
];


const INSTRUCTOR_ROUTES = ['/instructor'];
const ADMIN_ROUTES = ['/admin'];


function isPublicRoute(pathname: string): boolean {
  if (PUBLIC_ROUTES.includes(pathname)) {
    return true;
  }
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
    const decodedValue = decodeURIComponent(userCookie.value);
    return JSON.parse(decodedValue);
  } catch (error) {
    console.error('Error parsing user cookie:', error);
    return null
  }
}

function hasValidToken(request: NextRequest): boolean {
  const accessToken = request.cookies.get('accessToken');
  const localStorageToken = request.cookies.get('accessTokenFromStorage');
  return !!(accessToken || localStorageToken);
}



export function middleware(request: NextRequest) {
  const { pathname } = request.nextUrl;


  if (
    pathname.startsWith('/_next') ||
    pathname.startsWith('/api') ||
    pathname.includes('.')
  ) {
    return NextResponse.next();
  }

  if (isPublicRoute(pathname)) {
    return NextResponse.next();
  }


  if (isProtectedRoute(pathname)) {
    const hasToken = hasValidToken(request);

    if (!hasToken) {

      const loginUrl = new URL('/auth/login', request.url);
      loginUrl.searchParams.set('redirect', pathname);
      return NextResponse.redirect(loginUrl);
    }


    const user = getUserFromCookie(request);


    if (pathname.startsWith('/instructor')) {
      if (!user || (user.role !== 3 && user.role !== 1)) {
        return NextResponse.redirect(new URL('/courses', request.url));
      }
    }

    if (pathname.startsWith('/admin')) {
      if (!user || user.role !== 1) {
        return NextResponse.redirect(new URL('/courses', request.url));
      }
    }

    return NextResponse.next();
  }

  return NextResponse.next();
}

export const config = {
  matcher: [
    '/((?!_next/static|_next/image|favicon.ico|.*\\..*|public).*)',
  ],
};
