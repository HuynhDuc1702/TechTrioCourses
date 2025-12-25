// API URLs for microservices architecture
export const API_URLS = {
    ACCOUNT: process.env.NEXT_PUBLIC_ACCOUNT_API_URL || "https://localhost:7240",
    COURSE: process.env.NEXT_PUBLIC_COURSE_API_URL || "https://localhost:7102",
    USER: process.env.NEXT_PUBLIC_USER_API_URL || "https://localhost:7012",
    CATEGORY: process.env.NEXT_PUBLIC_CATEGORY_API_URL || "https://localhost:7273",
    LESSON: process.env.NEXT_PUBLIC_LESSON_API_URL || "https://localhost:7088",

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

    USER_COURSES: {
        BASE: '/api/UserCourses',
        GET_BY_ID: (id: string) => `/api/UserCourses/${id}`,
        BY_USER: (userId: string) => `/api/UserCourses/by-user/${userId}`,
        BY_COURSE: (courseId: string) => `/api/UserCourses/by-course/${courseId}`,
        BY_USER_AND_COURSE: (userId: string, courseId: string) => `/api/UserCourses/by-user-and-course/${userId}/${courseId}`,
        IS_ENROLLED: (userId: string, courseId: string) => `/api/UserCourses/is-enrolled/${userId}/${courseId}`,
        RECALCULATE_PROGRESS: (id: string) => `/api/UserCourses/${id}`,
    },

    USER_LESSONS: {
        BASE: '/api/UserLessons',
        GET_BY_ID: (id: string) => `/api/UserLessons/${id}`,
        BY_USER: (userId: string) => `/api/UserLessons/by-user/${userId}`,
        BY_LESSON: (lessonId: string) => `/api/UserLessons/by-lesson/${lessonId}`,
        BY_USER_AND_LESSON: (userId: string, lessonId: string) => `/api/UserLessons/by-user-and-lesson/${userId}/${lessonId}`,
        IS_COMPLETED: (userId: string, lessonId: string) => `/api/UserLessons/is-completed/${userId}/${lessonId}`,
    },

    USER_QUIZZES: {
        BASE: '/api/UserQuizzes',
        GET_BY_ID: (id: string) => `/api/UserQuizzes/${id}`,
        BY_USER: (userId: string) => `/api/UserQuizzes/by-user/${userId}`,
        BY_QUIZ: (quizId: string) => `/api/UserQuizzes/by-quiz/${quizId}`,
        BY_COURSE: (courseId: string) => `/api/UserQuizzes/by-course/${courseId}`,
        BY_USER_AND_QUIZ: (userId: string, quizId: string) => `/api/UserQuizzes/by-user-and-quiz/${userId}/${quizId}`,
        BY_USER_AND_COURSE: (userId: string, courseId: string) => `/api/UserQuizzes/by-user-and-course/${userId}/${courseId}`,
    },

    CATEGORIES: {
        BASE: '/api/Categories',
    },
    LESSONS: {
        BASE: '/api/Lessons',
    },
}