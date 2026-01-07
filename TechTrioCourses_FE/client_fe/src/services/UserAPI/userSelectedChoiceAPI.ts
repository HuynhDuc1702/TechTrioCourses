import { API_ENDPOINTS } from '@/constants/apiURL';
import { userAxios } from '@/middleware/axiosMiddleware';

export interface UserSelectedChoiceCreateRequest {
    resultId: string;
    questionId: string;
    choiceId: string;
}

export interface UserSelectedChoiceUpdateRequest {
    resultId?: string;
    questionId?: string;
    choiceId?: string;
}

export interface UserSelectedChoiceResponse {
    id: string;
    resultId: string;
    questionId: string;
    choiceId: string;
    createdAt: string;
    updatedAt: string;
}

// ==================== USER SELECTED CHOICE API ====================

export const userSelectedChoiceAPI = {
    /**
     * Get all user selected choices
     * GET: /api/UserSelectedChoices
     */
    getAllUserSelectedChoices: async (): Promise<UserSelectedChoiceResponse[]> => {
        const response = await userAxios.get<UserSelectedChoiceResponse[]>(API_ENDPOINTS.USER_SELECTED_CHOICES.BASE);
        return response.data;
    },

    /**
     * Get user selected choice by ID
     * GET: /api/UserSelectedChoices/{id}
     */
    getUserSelectedChoiceById: async (id: string): Promise<UserSelectedChoiceResponse> => {
        const response = await userAxios.get<UserSelectedChoiceResponse>(
            API_ENDPOINTS.USER_SELECTED_CHOICES.GET_BY_ID(id)
        );
        return response.data;
    },

    /**
     * Get user selected choices by result ID
     * GET: /api/UserSelectedChoices/result/{resultId}
     */
    getUserSelectedChoicesByResult: async (resultId: string): Promise<UserSelectedChoiceResponse[]> => {
        const response = await userAxios.get<UserSelectedChoiceResponse[]>(
            API_ENDPOINTS.USER_SELECTED_CHOICES.BY_RESULT(resultId)
        );
        return response.data;
    },

    /**
     * Get user selected choice by result and question
     * GET: /api/UserSelectedChoices/result/{resultId}/question/{questionId}
     */
    getUserSelectedChoiceByResultAndQuestion: async (resultId: string, questionId: string): Promise<UserSelectedChoiceResponse> => {
        const response = await userAxios.get<UserSelectedChoiceResponse>(
            API_ENDPOINTS.USER_SELECTED_CHOICES.BY_RESULT_AND_QUESTION(resultId, questionId)
        );
        return response.data;
    },

    /**
     * Create new user selected choice
     * POST: /api/UserSelectedChoices
     */
    createUserSelectedChoice: async (data: UserSelectedChoiceCreateRequest): Promise<UserSelectedChoiceResponse> => {
        const response = await userAxios.post<UserSelectedChoiceResponse>(
            API_ENDPOINTS.USER_SELECTED_CHOICES.BASE,
            data
        );
        return response.data;
    },

    /**
     * Update user selected choice
     * PUT: /api/UserSelectedChoices/{id}
     */
    updateUserSelectedChoice: async (id: string, data: UserSelectedChoiceUpdateRequest): Promise<UserSelectedChoiceResponse> => {
        const response = await userAxios.put<UserSelectedChoiceResponse>(
            API_ENDPOINTS.USER_SELECTED_CHOICES.GET_BY_ID(id),
            data
        );
        return response.data;
    },

    /**
     * Delete user selected choice
     * DELETE: /api/UserSelectedChoices/{id}
     */
    deleteUserSelectedChoice: async (id: string): Promise<void> => {
        await userAxios.delete(API_ENDPOINTS.USER_SELECTED_CHOICES.GET_BY_ID(id));
    },
};
