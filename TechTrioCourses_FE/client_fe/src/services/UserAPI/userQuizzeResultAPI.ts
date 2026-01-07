import { API_ENDPOINTS } from '@/constants/apiURL';
import { userAxios } from '@/middleware/axiosMiddleware';

export enum UserQuizzeResultStatusEnum {

  InProgress = 1,
  Completed = 2
}

export interface UserQuizzeResultCreateRequest {
  userId: string;
  courseId: string;
  quizId: string;
  attemptNumber: number;
  userquizzeResultStatus: UserQuizzeResultStatusEnum;
  metadata?: string;
}
export interface UserQuizzeResultUpdateRequest {

  score?: number;
  userquizzeResultStatus?: UserQuizzeResultStatusEnum;
  completedAt?: string;
  durationSeconds?: number;
  metadata?: string;
}
export interface UserQuizzeResultResponse {
  id: string;
  userId: string;
  courseId: string;
  quizId: string;
  score?: number;
  userquizzeResultStatus: UserQuizzeResultStatusEnum;
  attemptNumber: number;
  metadata?: string;
  startedAt: string;
  completedAt: string;
  durationSeconds?: number;
  updatedAt: string;
}
// ==================== USER QUIZ API ====================

export const userQuizzeResultsAPI = {
  /**
   * Get all user quizzes results
   * GET: /api/UserQuizzeResults
   */
  getAllUserQuizzeResults: async (): Promise<UserQuizzeResultResponse[]> => {
    const response = await userAxios.get<UserQuizzeResultResponse[]>(API_ENDPOINTS.USER_QUIZZE_RESULTS.BASE);
    return response.data;
  },

  /**
   * Get user quizze result by ID
   * GET: /api/UserQuizzeResults/{id}
   */
  getUserQuizzeResultById: async (id: string): Promise<UserQuizzeResultResponse> => {
    const response = await userAxios.get<UserQuizzeResultResponse>(
      API_ENDPOINTS.USER_QUIZZE_RESULTS.GET_BY_ID(id)
    );
    return response.data;
  },

  /**
   * Get user quizze result by user ID
   * GET: /api/UserQuizzeResults/by-user
   */
  getUserQuizzeResultsByUserId: async (): Promise<UserQuizzeResultResponse[]> => {
    const response = await userAxios.get<UserQuizzeResultResponse[]>(
      API_ENDPOINTS.USER_QUIZZE_RESULTS.BY_USER()
    );
    return response.data;
  },

  /**
   * Get user quizze result by quiz ID
   * GET: /api/UserQuizzeResults/by-quiz/{quizId}
   */
  getUserQuizzeResultsByQuizId: async (quizId: string): Promise<UserQuizzeResultResponse[]> => {
    const response = await userAxios.get<UserQuizzeResultResponse[]>(
      API_ENDPOINTS.USER_QUIZZE_RESULTS.BY_QUIZ(quizId)
    );
    return response.data;
  },

  /**
   * Get user quizze result by course ID
   * GET: /api/UserQuizzeResults/by-course/{courseId}
   */
  getUserQuizzeResultsByCourseId: async (courseId: string): Promise<UserQuizzeResultResponse[]> => {
    const response = await userAxios.get<UserQuizzeResultResponse[]>(
      API_ENDPOINTS.USER_QUIZZE_RESULTS.BY_COURSE(courseId)
    );
    return response.data;
  },

  /**
   * Get user quizze result by user and quiz
   * GET: /api/UserQuizzeResults/by-user-and-quiz/{quizId}
   */
  getUserQuizzeResultByUserAndQuiz: async (quizId: string): Promise<UserQuizzeResultResponse> => {
    const response = await userAxios.get<UserQuizzeResultResponse>(
      API_ENDPOINTS.USER_QUIZZE_RESULTS.BY_USER_AND_QUIZ(quizId)
    );
    return response.data;
  },

  /**
   * Get user quizze result by user and course
   * GET: /api/UserQuizzeResults/by-user-and-course/{courseId}
   */
  getUserQuizzeResultsByUserAndCourse: async (courseId: string): Promise<UserQuizzeResultResponse[]> => {
    const response = await userAxios.get<UserQuizzeResultResponse[]>(
      API_ENDPOINTS.USER_QUIZZE_RESULTS.BY_USER_AND_COURSE(courseId)
    );
    return response.data;
  },

  /**
   * Create new user result quiz
   * POST: /api/UserQuizzeResults
   */
  createUserQuizzeResult: async (data: UserQuizzeResultCreateRequest): Promise<UserQuizzeResultResponse> => {
    const response = await userAxios.post<UserQuizzeResultResponse>(
      API_ENDPOINTS.USER_QUIZZE_RESULTS.BASE,
      data
    );
    return response.data;
  },

  /**
   * Update user quizze result
   * PUT: /api/UserQuizzeResults/{id}
   */
  updateUserQuizzeResult: async (id: string, data: UserQuizzeResultUpdateRequest): Promise<UserQuizzeResultResponse> => {
    const response = await userAxios.put<UserQuizzeResultResponse>(
      API_ENDPOINTS.USER_QUIZZE_RESULTS.GET_BY_ID(id),
      data
    );
    return response.data;
  },

  /**
   * Delete user quizze result
   * DELETE: /api/UserQuizzeResults/{id}
   */
  deleteUserQuizzeResult: async (id: string): Promise<void> => {
    await userAxios.delete(API_ENDPOINTS.USER_QUIZZE_RESULTS.GET_BY_ID(id));
  },
};