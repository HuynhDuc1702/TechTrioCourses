// API Gateway - Single entry point for all backend services
export const API_URL = process.env.NEXT_PUBLIC_API_GATEWAY_URL || "https://localhost:5000";

// API URLs for microservices (all now route through gateway)
export const API_URLS = {
    ACCOUNT: API_URL,
    COURSE: API_URL,
    USER: API_URL,
    CATEGORY: API_URL,
    LESSON: API_URL,
    QUIZ: API_URL,
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
        BY_USER: () => `/api/UserCourses/by-user`,
        BY_COURSE: (courseId: string) => `/api/UserCourses/by-course/${courseId}`,
        BY_USER_AND_COURSE: (courseId: string) => `/api/UserCourses/by-user-and-course/${courseId}`,
        IS_ENROLLED: (courseId: string) => `/api/UserCourses/is-enrolled/${courseId}`,
    },

    USER_LESSONS: {
        BASE: '/api/UserLessons',
        GET_BY_ID: (id: string) => `/api/UserLessons/${id}`,
        BY_USER: () => `/api/UserLessons/by-user`,
        BY_LESSON: (lessonId: string) => `/api/UserLessons/by-lesson/${lessonId}`,
        BY_USER_AND_LESSON: (lessonId: string) => `/api/UserLessons/by-user-and-lesson/${lessonId}`,
        IS_COMPLETED: (lessonId: string) => `/api/UserLessons/is-completed/${lessonId}`,

    },

    USER_QUIZZES: {
        BASE: '/api/UserQuizzes',
        GET_BY_ID: (id: string) => `/api/UserQuizzes/${id}`,
        BY_USER: () => `/api/UserQuizzes/by-user`,
        BY_QUIZ: (quizId: string) => `/api/UserQuizzes/by-quiz/${quizId}`,
        BY_COURSE: (courseId: string) => `/api/UserQuizzes/by-course/${courseId}`,
        BY_USER_AND_QUIZ: (quizId: string) => `/api/UserQuizzes/by-user-and-quiz/${quizId}`,
        BY_USER_AND_COURSE: (courseId: string) => `/api/UserQuizzes/by-user-and-course/${courseId}`,
        IS_PASSED: (quizId: string) => `/api/UserQuizzes/is-passed/${quizId}`,
    },

    CATEGORIES: {
        BASE: '/api/Categories',
    },
    LESSONS: {
        BASE: '/api/Lessons',
    },
    QUIZZES: {
        BASE: '/api/Quizzes',
    },
    QUESTIONS: {
        BASE: '/api/Questions',
    },
    QUESTION_CHOICES: {
        BASE: '/api/QuestionChoices',
    },
    QUESTION_ANSWERS: {
        BASE: '/api/QuestionAnswers',
    },
    QUIZ_QUESTIONS: {
        BASE: '/api/QuizQuestions',
    },
    USER_SELECTED_CHOICES: {
        BASE: '/api/UserSelectedChoices',
        GET_BY_ID: (id: string) => `/api/UserSelectedChoices/${id}`,
        BY_RESULT: (resultId: string) => `/api/UserSelectedChoices/result/${resultId}`,
        BY_RESULT_AND_QUESTION: (resultId: string, questionId: string) => `/api/UserSelectedChoices/result/${resultId}/question/${questionId}`,
    },
    USER_INPUT_ANSWERS: {
        BASE: '/api/UserInputAnswers',
        GET_BY_ID: (id: string) => `/api/UserInputAnswers/${id}`,
        BY_RESULT: (resultId: string) => `/api/UserInputAnswers/result/${resultId}`,
        BY_RESULT_AND_QUESTION: (resultId: string, questionId: string) => `/api/UserInputAnswers/result/${resultId}/question/${questionId}`,
    },
    USER_QUIZZE_RESULTS: {
        BASE: '/api/UserQuizzeResults',
        GET_BY_ID: (id: string) => `/api/UserQuizzeResults/${id}`,
        BY_USER: () => `/api/UserQuizzeResults/by-user`,
        BY_QUIZ: (quizId: string) => `/api/UserQuizzeResults/by-quiz/${quizId}`,
        BY_COURSE: (courseId: string) => `/api/UserQuizzeResults/by-course/${courseId}`,
        BY_USER_AND_QUIZ: (quizId: string) => `/api/UserQuizzeResults/by-user-and-quiz/${quizId}`,
        BY_USER_AND_COURSE: (courseId: string) => `/api/UserQuizzeResults/by-user-and-course/${courseId}`,
    },
}