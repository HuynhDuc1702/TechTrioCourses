// API URLs for microservices architecture
export const API_URLS = {
    ACCOUNT: process.env.NEXT_PUBLIC_ACCOUNT_API_URL || "https://localhost:7240",
    COURSE: process.env.NEXT_PUBLIC_COURSE_API_URL || "https://localhost:7102",
    USER: process.env.NEXT_PUBLIC_USER_API_URL || "https://localhost:7012",
    CATEGORY: process.env.NEXT_PUBLIC_CATEGORY_API_URL || "https://localhost:7273",
}

// API endpoints (paths only, base URL comes from API_URLS)
export const API_ENDPOINTS = {
    COURSES: {
        BASE: '/api/Courses',
    },

    ACCOUNTS: {
        LOGIN: '/api/Accounts/login',
        REGISTER: '/api/Accounts/register',
        REFRESH_TOKEN: '/api/Accounts/refresh-token',
        SEND_OTP: '/api/Accounts/send-otp',
        VERIFY_OTP: '/api/Accounts/verify-otp',
        CHANGE_PASSWORD: '/api/Accounts/change-password',
        RESET_PASSWORD: '/api/Accounts/reset-password',
    },
    
    USERS: {
        BASE: '/api/Users',
        BY_ACCOUNT: (accountId: string) => `/api/Users/by-account/${accountId}`,
        GET_BY_IDS: '/api/Users/get-by-ids',
        GET_BY_ID: (id: string) => `/api/Users/${id}`,
    },
    CATEGORIES:{
        BASE: '/api/Categories',
    }
}