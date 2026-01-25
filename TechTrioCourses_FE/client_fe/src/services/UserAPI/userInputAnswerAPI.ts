import { API_ENDPOINTS } from '@/constants/apiURL';
import { userAxios } from '@/middleware/axiosMiddleware';

export interface UserInputAnswerCreateRequest {
    resultId: string;
    questionId: string;
    textAnswer: string;
}

export interface UserInputAnswerUpdateRequest {
    resultId?: string;
    questionId?: string;
    textAnswer?: string;
}

export interface UserInputAnswerResponse {
    id: string;
    resultId: string;
    questionId: string;
    textAnswer: string;
    createdAt: string;
    updatedAt: string;
}

// ==================== USER INPUT ANSWER API ====================

export const userInputAnswerAPI = {
    /**
     * Get all user input answers
     * GET: /api/UserInputAnswers
     */
    getAllUserInputAnswers: async (): Promise<UserInputAnswerResponse[]> => {
        const response = await userAxios.get<UserInputAnswerResponse[]>(API_ENDPOINTS.USER_INPUT_ANSWERS.BASE);
        return response.data;
    },

    /**
     * Get user input answer by ID
     * GET: /api/UserInputAnswers/{id}
     */
    getUserInputAnswerById: async (id: string): Promise<UserInputAnswerResponse> => {
        const response = await userAxios.get<UserInputAnswerResponse>(
            API_ENDPOINTS.USER_INPUT_ANSWERS.GET_BY_ID(id)
        );
        return response.data;
    },

    /**
     * Get user input answers by result ID
     * GET: /api/UserInputAnswers/result/{resultId}
     */
    getUserInputAnswersByResult: async (resultId: string): Promise<UserInputAnswerResponse[]> => {
        const response = await userAxios.get<UserInputAnswerResponse[]>(
            API_ENDPOINTS.USER_INPUT_ANSWERS.BY_RESULT(resultId)
        );
        return response.data;
    },

    /**
     * Get user input answer by result and question
     * GET: /api/UserInputAnswers/result/{resultId}/question/{questionId}
     */
    getUserInputAnswerByResultAndQuestion: async (resultId: string, questionId: string): Promise<UserInputAnswerResponse> => {
        const response = await userAxios.get<UserInputAnswerResponse>(
            API_ENDPOINTS.USER_INPUT_ANSWERS.BY_RESULT_AND_QUESTION(resultId, questionId)
        );
        return response.data;
    },

    /**
     * Create new user input answer
     * POST: /api/UserInputAnswers
     */
    createUserInputAnswer: async (data: UserInputAnswerCreateRequest): Promise<UserInputAnswerResponse> => {
        const response = await userAxios.post<UserInputAnswerResponse>(
            API_ENDPOINTS.USER_INPUT_ANSWERS.BASE,
            data
        );
        return response.data;
    },

    /**
     * Update user input answer
     * PUT: /api/UserInputAnswers/{id}
     */
    updateUserInputAnswer: async (id: string, data: UserInputAnswerUpdateRequest): Promise<UserInputAnswerResponse> => {
        const response = await userAxios.put<UserInputAnswerResponse>(
            API_ENDPOINTS.USER_INPUT_ANSWERS.GET_BY_ID(id),
            data
        );
        return response.data;
    },

    /**
     * Delete user input answer
     * DELETE: /api/UserInputAnswers/{id}
     */
    deleteUserInputAnswer: async (id: string): Promise<void> => {
        await userAxios.delete(API_ENDPOINTS.USER_INPUT_ANSWERS.GET_BY_ID(id));
    },
};
