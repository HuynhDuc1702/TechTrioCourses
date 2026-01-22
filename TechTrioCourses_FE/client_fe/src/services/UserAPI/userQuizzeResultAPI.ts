import { API_ENDPOINTS } from '@/constants/apiURL';
import { userAxios } from '@/middleware/axiosMiddleware';
import { QuestionTypeEnum } from '../quizAPI';

export enum UserQuizzeResultStatusEnum {

  InProgress = 1,
  Completed = 2
}

export interface UserQuizzeResultCreateRequest {
  userId: string;
  courseId: string;
  quizId: string;
  userQuizId: string;
 

}
export interface UserQuizzeResultUpdateRequest {

  score?: number | null;
  userquizzeResultStatus?: UserQuizzeResultStatusEnum;
  completedAt?: string;
  durationSeconds?: number | null;
  metadata?: string;
}
export interface UserQuizzeResultResponse {
  id: string;
  userId: string;
  courseId: string;
  quizId: string;
  score?: number | null;
  userquizzeResultStatus: UserQuizzeResultStatusEnum;
  attemptNumber: number;
  metadata?: string;
  startedAt: string;
  completedAt?: string | null; 
  durationSeconds?: number | null;
  updatedAt: string;
}
//Get user quizze result including user answers and selected choices
export interface UserQuizzeResultQuestionAnswer {
  questionId: string;
  textAnswer?: string | null;
  selectedChoiceIds?: string[] | null;
}
export interface UserQuizzeResultResumeResponse {
  resultId: string;
  quizId: string;
  userQuizId: string;
  attemptNumber: number;
  startedAt: string;
  answers: UserQuizzeResultQuestionAnswer[];
}
export interface UserQuizzeResultReviewResponse {
  resultId: string;
  quizId: string;
  userQuizId: string;
  attemptNumber: number;
  score?: number | null;
  startedAt: string;
  completedAt?: string | null;
  durationSeconds?: number | null;
  status: UserQuizzeResultStatusEnum;
  metadata?: string | null;
  answers: UserQuizzeResultQuestionAnswer[];
}
//Submit user quizze result including user answers and selected choices
export interface SubmitQuizRequestDto{
  resultId: string;
  userquizId: string;
  durationSeconds?: number | null;
  IsFinalSubmission: boolean;
  answers: QuestionAnswersDtos[];

}
export interface QuestionAnswersDtos{
  questionId: string;
  questionType: QuestionTypeEnum;
  selectedChoiceIds?: string[] | null;
  inputAnswer?: string | null;
}
export interface SubmitQuizResponseDto{
  resultId: string;
  userquizId: string;
  message?: string | null;
  status: UserQuizzeResultStatusEnum;
  score?: number | null;
  ispassed?: boolean | null;
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
   * Get user quizze result by userQuiz ID
   * GET: /api/UserQuizzeResults/by-course/{courseId}
   */
  getUserQuizzeResultsByUserQuizId: async (userQuizId: string): Promise<UserQuizzeResultResponse[]> => {
    const response = await userAxios.get<UserQuizzeResultResponse[]>(
      API_ENDPOINTS.USER_QUIZZE_RESULTS.BY_USER_QUIZ(userQuizId)
    );
    return response.data;
  },
  
  /**
   * Get latest user quizze result by userQuiz ID
   * GET: /api/UserQuizzeResults/by-course/{courseId}
   */
  getLatestUserQuizzeResultByUserQuizId: async (userQuizId: string): Promise<UserQuizzeResultResponse> => {
    const response = await userAxios.get<UserQuizzeResultResponse>(
      API_ENDPOINTS.USER_QUIZZE_RESULTS.GET_LATEST(userQuizId)
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
   * Review user quizze result
   * GET: /api/UserQuizzeResults/review/{id}  
    */
  reviewUserQuizzeResult: async (id: string): Promise<UserQuizzeResultReviewResponse> => {
    const response = await userAxios.get<UserQuizzeResultReviewResponse>(
      API_ENDPOINTS.USER_QUIZZE_RESULTS.REVIEW(id)
    );
    return response.data;
  },
  /**
   * Resume user quizze result
   * GET: /api/UserQuizzeResults/resume/{id}
  */
   resumeUserQuizzeResult: async (id: string): Promise<UserQuizzeResultResumeResponse> => {
    const response = await userAxios.get<UserQuizzeResultResumeResponse>(
      API_ENDPOINTS.USER_QUIZZE_RESULTS.RESUME(id)
    );
    return response.data;
  },
  /**
   * Submit user quizze result
   * POST: /api/UserQuizzeResults/submit/{id}
   */
   submitUserQuizzeResult: async (id: string, data: SubmitQuizRequestDto): Promise<SubmitQuizResponseDto> => {
    const response = await userAxios.post<SubmitQuizResponseDto>(
      API_ENDPOINTS.USER_QUIZZE_RESULTS.SUBMIT(id),
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