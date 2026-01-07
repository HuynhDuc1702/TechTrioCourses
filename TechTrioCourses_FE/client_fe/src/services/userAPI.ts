import { API_ENDPOINTS } from '@/constants/apiURL';
import { userAxios } from '@/middleware/axiosMiddleware';

// ==================== INTERFACES ====================

// User Enums
export enum UserRoleEnum {
  Admin = 1,
  Student = 2,
  Instructor = 3
}

export enum UserCourseStatus {
  Dropped = 1,
  In_progress = 2,
  Completed = 3
}

export enum UserLessonStatus {
  Not_Started = 1,
  Completed = 2
}

export enum UserQuizStatus {
  Not_Started = 1,
  In_progress = 2,
  Passed = 3,
  Failed = 4
}

// User Interfaces
export interface UserResponse {
  id: string;
  accountId: string;
  fullName: string;
  avatarUrl?: string;
  role: UserRoleEnum;
  createdAt: string;
}

export interface CreateUserRequest {
  accountId: string;
  fullName: string;
  avatarUrl?: string;
  role?: UserRoleEnum;
}

export interface UpdateUserRequest {
  fullName?: string;
  avatarUrl?: string;
  role?: UserRoleEnum;
}

// UserCourse Interfaces
export interface UserCourseResponse {
  id: string;
  userId: string;
  courseId: string;
  status: UserCourseStatus;
  progress: number;
  enrolledAt: string;
  completedAt?: string;
  updatedAt: string;
}

export interface CreateUserCourseRequest {
  userId: string;
  courseId: string;
}

export interface UpdateUserCourseRequest {
  status?: UserCourseStatus;
  completedAt?: string;
}

// UserLesson Interfaces
export interface UserLessonResponse {
  id: string;
  userId: string;
  lessonId: string;
  status: UserLessonStatus;
  completedAt?: string;
  updatedAt: string;
}

export interface CreateUserLessonRequest {
  userId: string;
  lessonId: string;
  courseId: string;
}

export interface UpdateUserLessonRequest {
  status?: UserLessonStatus;
  completedAt?: string;
}

// UserQuiz Interfaces
export interface UserQuizResponse {
  id: string;
  userId: string;
  quizId: string;
  status: UserQuizStatus;
  attemptCount: number;
  bestScore?: number;
  firstAttemptAt?: string;
  lastAttemptAt?: string;
  passedAt?: string;
  updatedAt: string;
}

export interface CreateUserQuizRequest {
  userId: string;
  quizId: string;
  attemptCount: number;
  bestScore?: number;
}

export interface UpdateUserQuizRequest {
  status?: UserQuizStatus;
  attemptCount?: number;
  bestScore?: number;
  firstAttemptAt?: string;
  lastAttemptAt?: string;
  passedAt?: string;
}

// ==================== USER SERVICE ====================

export const userAPI = {
  /**
   * Get all users
   * GET: /api/Users
   */
  getAllUsers: async (): Promise<UserResponse[]> => {
    const response = await userAxios.get<UserResponse[]>(API_ENDPOINTS.USERS.BASE);
    return response.data;
  },

  /**
   * Get user by ID
   * GET: /api/Users/{id}
   */
  getUserById: async (id: string): Promise<UserResponse> => {
    const response = await userAxios.get<UserResponse>(
      API_ENDPOINTS.USERS.GET_BY_ID(id)
    );
    return response.data;
  },

  /**
   * Get user by account ID
   * GET: /api/Users/by-account/{accountId}
   */
  getUserByAccountId: async (accountId: string): Promise<UserResponse> => {
    const response = await userAxios.get<UserResponse>(
      `${API_ENDPOINTS.USERS.BASE}/by-account/${accountId}`
    );
    return response.data;
  },

  /**
   * Get users by multiple IDs
   * POST: /api/Users/get-by-ids
   */
  getUsersByIds: async (ids: string[]): Promise<UserResponse[]> => {
    const response = await userAxios.post<UserResponse[]>(
      `${API_ENDPOINTS.USERS.BASE}/get-by-ids`,
      ids
    );
    return response.data;
  },

  /**
   * Create new user
   * POST: /api/Users
   */
  createUser: async (data: CreateUserRequest): Promise<UserResponse> => {
    const response = await userAxios.post<UserResponse>(
      API_ENDPOINTS.USERS.BASE,
      data
    );
    return response.data;
  },

  /**
   * Update user by ID
   * PUT: /api/Users/{id}
   */
  updateUser: async (id: string, data: UpdateUserRequest): Promise<UserResponse> => {
    const response = await userAxios.put<UserResponse>(
      `${API_ENDPOINTS.USERS.BASE}/${id}`,
      data
    );
    return response.data;
  },

  /**
   * Get current user profile (helper method)
   */
  getCurrentUserProfile: async (): Promise<UserResponse | null> => {
    try {
      const user = localStorage.getItem('user');
      if (user) {
        const parsedUser = JSON.parse(user);
        return await userAPI.getUserById(parsedUser.userId);
      }
      return null;
    } catch (error) {
      console.error('Error getting current user profile:', error);
      return null;
    }
  },

  /**
   * Update current user profile (helper method)
   */
  updateCurrentUserProfile: async (data: UpdateUserRequest): Promise<UserResponse | null> => {
    try {
      const user = localStorage.getItem('user');
      if (user) {
        const parsedUser = JSON.parse(user);
        const updatedUser = await userAPI.updateUser(parsedUser.userId, data);

        // Update local storage
        localStorage.setItem('user', JSON.stringify({
          ...parsedUser,
          fullName: updatedUser.fullName,
          avatarUrl: updatedUser.avatarUrl,
          role: updatedUser.role
        }));

        return updatedUser;
      }
      return null;
    } catch (error) {
      console.error('Error updating current user profile:', error);
      return null;
    }
  },

  /**
   * Check if current user has specific role
   */
  hasRole: (role: UserRoleEnum): boolean => {
    try {
      const user = localStorage.getItem('user');
      if (user) {
        const parsedUser = JSON.parse(user);
        return parsedUser.role === role;
      }
      return false;
    } catch (error) {
      return false;
    }
  },

  /**
   * Check if current user is admin
   */
  isAdmin: (): boolean => {
    return userAPI.hasRole(UserRoleEnum.Admin);
  },

  /**
   * Check if current user is instructor
   */
  isInstructor: (): boolean => {
    return userAPI.hasRole(UserRoleEnum.Instructor);
  },

  /**
   * Check if current user is student
   */
  isStudent: (): boolean => {
    return userAPI.hasRole(UserRoleEnum.Student);
  },
};

// ==================== USER COURSE SERVICE ====================

export const userCourseAPI = {
  /**
   * Get all user courses
   * GET: /api/UserCourses
   */
  getAllUserCourses: async (): Promise<UserCourseResponse[]> => {
    const response = await userAxios.get<UserCourseResponse[]>(API_ENDPOINTS.USER_COURSES.BASE);
    return response.data;
  },

  /**
   * Get user course by ID
   * GET: /api/UserCourses/{id}
   */
  getUserCourseById: async (id: string): Promise<UserCourseResponse> => {
    const response = await userAxios.get<UserCourseResponse>(
      API_ENDPOINTS.USER_COURSES.GET_BY_ID(id)
    );
    return response.data;
  },

  /**
   * Get user courses for current user
   * GET: /api/UserCourses/by-user
   */
  getMyUserCourses: async (): Promise<UserCourseResponse[]> => {
    const response = await userAxios.get<UserCourseResponse[]>(
      API_ENDPOINTS.USER_COURSES.BY_USER()
    );
    return response.data;
  },

  /**
   * Get user courses by course ID
   * GET: /api/UserCourses/by-course/{courseId}
   */
  getUserCoursesByCourseId: async (courseId: string): Promise<UserCourseResponse[]> => {
    const response = await userAxios.get<UserCourseResponse[]>(
      API_ENDPOINTS.USER_COURSES.BY_COURSE(courseId)
    );
    return response.data;
  },

  /**
   * Get user course by user and course
   * GET: /api/UserCourses/by-user-and-course/{courseId}
   */
  getUserCourseByUserAndCourse: async (courseId: string): Promise<UserCourseResponse> => {
    const response = await userAxios.get<UserCourseResponse>(
      API_ENDPOINTS.USER_COURSES.BY_USER_AND_COURSE(courseId)
    );
    return response.data;
  },

  /**
   * Check if user is enrolled in course
   * GET: /api/UserCourses/is-enrolled/{userId}/{courseId}
   */
  checkIsEnrolled: async (courseId: string): Promise<{ isEnrolled: boolean }> => {
    const response = await userAxios.get<{ isEnrolled: boolean }>(
      API_ENDPOINTS.USER_COURSES.IS_ENROLLED(courseId)
    );
    return response.data;
  },

  /**
   * Create new user course (enroll)
   * POST: /api/UserCourses
   */
  createUserCourse: async (data: CreateUserCourseRequest): Promise<UserCourseResponse> => {
    const response = await userAxios.post<UserCourseResponse>(
      API_ENDPOINTS.USER_COURSES.BASE,
      data
    );
    return response.data;
  },



  /**
   * Delete user course (unenroll)
   * DELETE: /api/UserCourses/{id}
   */
  deleteUserCourse: async (id: string): Promise<void> => {
    await userAxios.delete(API_ENDPOINTS.USER_COURSES.GET_BY_ID(id));
  },
};

// ==================== USER LESSON API ====================

export const userLessonAPI = {
  /**
   * Get all user lessons
   * GET: /api/UserLessons
   */
  getAllUserLessons: async (): Promise<UserLessonResponse[]> => {
    const response = await userAxios.get<UserLessonResponse[]>(API_ENDPOINTS.USER_LESSONS.BASE);
    return response.data;
  },

  /**
   * Get user lesson by ID
   * GET: /api/UserLessons/{id}
   */
  getUserLessonById: async (id: string): Promise<UserLessonResponse> => {
    const response = await userAxios.get<UserLessonResponse>(
      API_ENDPOINTS.USER_LESSONS.GET_BY_ID(id)
    );
    return response.data;
  },

  /**
   * Get user lessons by user ID
   * GET: /api/UserLessons/by-user
   */
  getUserLessonsByUserId: async (): Promise<UserLessonResponse[]> => {
    const response = await userAxios.get<UserLessonResponse[]>(
      API_ENDPOINTS.USER_LESSONS.BY_USER()
    );
    return response.data;
  },

  /**
   * Get user lessons by lesson ID
   * GET: /api/UserLessons/by-lesson/{lessonId}
   */
  getUserLessonsByLessonId: async (lessonId: string): Promise<UserLessonResponse[]> => {
    const response = await userAxios.get<UserLessonResponse[]>(
      API_ENDPOINTS.USER_LESSONS.BY_LESSON(lessonId)
    );
    return response.data;
  },

  /**
   * Get user lesson by user and lesson
   * GET: /api/UserLessons/by-user-and-lesson/{lessonId}
   */
  getUserLessonByUserAndLesson: async (lessonId: string): Promise<UserLessonResponse> => {
    const response = await userAxios.get<UserLessonResponse>(
      API_ENDPOINTS.USER_LESSONS.BY_USER_AND_LESSON(lessonId)
    );
    return response.data;
  },

  /**
   * Create new user lesson
   * POST: /api/UserLessons
   */
  createUserLesson: async (data: CreateUserLessonRequest): Promise<UserLessonResponse> => {
    const response = await userAxios.post<UserLessonResponse>(
      API_ENDPOINTS.USER_LESSONS.BASE,
      data
    );
    return response.data;
  },

  /**
   * Check if user completed lesson
   * GET: /api/UserLessons/is-completed/{lessonId}
   */
  checkIsCompleted: async (lessonId: string): Promise<{ isCompleted: boolean }> => {
    const response = await userAxios.get<{ isCompleted: boolean }>(
      API_ENDPOINTS.USER_LESSONS.IS_COMPLETED(lessonId)
    );
    return response.data;
  },

  /**
   * Delete user lesson
   * DELETE: /api/UserLessons/{id}
   */
  deleteUserLesson: async (id: string): Promise<void> => {
    await userAxios.delete(API_ENDPOINTS.USER_LESSONS.GET_BY_ID(id));
  },
  //Mark lesson as completed
};

// ==================== USER QUIZ API ====================

export const userQuizAPI = {
  /**
   * Get all user quizzes
   * GET: /api/UserQuizzes
   */
  getAllUserQuizzes: async (): Promise<UserQuizResponse[]> => {
    const response = await userAxios.get<UserQuizResponse[]>(API_ENDPOINTS.USER_QUIZZES.BASE);
    return response.data;
  },

  /**
   * Get user quiz by ID
   * GET: /api/UserQuizzes/{id}
   */
  getUserQuizById: async (id: string): Promise<UserQuizResponse> => {
    const response = await userAxios.get<UserQuizResponse>(
      API_ENDPOINTS.USER_QUIZZES.GET_BY_ID(id)
    );
    return response.data;
  },

  /**
   * Get user quizzes by user ID
   * GET: /api/UserQuizzes/by-user
   */
  getUserQuizzesByUserId: async (): Promise<UserQuizResponse[]> => {
    const response = await userAxios.get<UserQuizResponse[]>(
      API_ENDPOINTS.USER_QUIZZES.BY_USER()
    );
    return response.data;
  },

  /**
   * Get user quizzes by quiz ID
   * GET: /api/UserQuizzes/by-quiz/{quizId}
   */
  getUserQuizzesByQuizId: async (quizId: string): Promise<UserQuizResponse[]> => {
    const response = await userAxios.get<UserQuizResponse[]>(
      API_ENDPOINTS.USER_QUIZZES.BY_QUIZ(quizId)
    );
    return response.data;
  },

  /**
   * Get user quizzes by course ID
   * GET: /api/UserQuizzes/by-course/{courseId}
   */
  getUserQuizzesByCourseId: async (courseId: string): Promise<UserQuizResponse[]> => {
    const response = await userAxios.get<UserQuizResponse[]>(
      API_ENDPOINTS.USER_QUIZZES.BY_COURSE(courseId)
    );
    return response.data;
  },

  /**
   * Get user quiz by user and quiz
   * GET: /api/UserQuizzes/by-user-and-quiz/{quizId}
   */
  getUserQuizByUserAndQuiz: async (quizId: string): Promise<UserQuizResponse> => {
    const response = await userAxios.get<UserQuizResponse>(
      API_ENDPOINTS.USER_QUIZZES.BY_USER_AND_QUIZ(quizId)
    );
    return response.data;
  },

  /**
   * Get user quizzes by user and course
   * GET: /api/UserQuizzes/by-user-and-course/{courseId}
   */
  getUserQuizzesByUserAndCourse: async (courseId: string): Promise<UserQuizResponse[]> => {
    const response = await userAxios.get<UserQuizResponse[]>(
      API_ENDPOINTS.USER_QUIZZES.BY_USER_AND_COURSE(courseId)
    );
    return response.data;
  },

  /**
   * Create new user quiz
   * POST: /api/UserQuizzes
   */
  createUserQuiz: async (data: CreateUserQuizRequest): Promise<UserQuizResponse> => {
    const response = await userAxios.post<UserQuizResponse>(
      API_ENDPOINTS.USER_QUIZZES.BASE,
      data
    );
    return response.data;
  },

  /**
   * Update user quiz
   * PUT: /api/UserQuizzes/{id}
   */
  updateUserQuiz: async (id: string, data: UpdateUserQuizRequest): Promise<UserQuizResponse> => {
    const response = await userAxios.put<UserQuizResponse>(
      API_ENDPOINTS.USER_QUIZZES.GET_BY_ID(id),
      data
    );
    return response.data;
  },

  /**
   * Delete user quiz
   * DELETE: /api/UserQuizzes/{id}
   */
  deleteUserQuiz: async (id: string): Promise<void> => {
    await userAxios.delete(API_ENDPOINTS.USER_QUIZZES.GET_BY_ID(id));
  },
  /**
   * Check if user passed quiz
   * GET: /api/UserQuizzes/is-passed/{quizId}
   */
  checkIsPassed: async (quizId: string): Promise<{ isPassed: boolean }> => {
    const response = await userAxios.get<{ isPassed: boolean }>(
      API_ENDPOINTS.USER_QUIZZES.IS_PASSED(quizId)
    );
    return response.data;
  },
};

export default userAPI;
