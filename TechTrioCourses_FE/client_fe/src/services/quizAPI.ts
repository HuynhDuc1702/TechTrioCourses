import { API_ENDPOINTS } from '@/constants/apiURL';
import { lessonAxios } from '@/middleware/axiosMiddleware';
// ==================== INTERFACES ====================
// Quiz related interfaces and enums
export enum QuizStatusEnum {
    Hidden = 1,
    Published = 2
}
export interface QuizCreateRequest {
    courseId: string;
    name: string;
    description?: string | null;
    totalMarks: number;
    durationMinutes: number;
    QuizStatus: QuizStatusEnum;
}
export interface QuizUpdateRequest {
    name?: string | null;
    description?: string | null;
    totalMarks?: number | null;
    durationMinutes?: number | null;
    QuizStatus?: QuizStatusEnum;
}
export interface QuizResponse {
    id: string;
    courseId: string;
    name: string;
    description?: string | null;
    totalMarks: number;
    durationMinutes: number;
    QuizStatus: QuizStatusEnum;
    createdAt: string;
    updatedAt: string;
}
// Question related interfaces and enums
export enum QuestionStatusEnum {
     Hidden = 1,
    Published = 2
}
export enum QuestionTypeEnum {
    MultipleChoice = 1,
    TrueFalse = 2,
    ShortAnswer = 3
}
export interface QuestionCreateRequest {
    userId: string;
    courseId: string;
    content: string;
    questionText : string;
    questionType: QuestionTypeEnum;
    points: number;
    status: QuestionStatusEnum;
}
export interface QuestionUpdateRequest {
    questionText? : string | null;
    points?: number | null;
    status?: QuestionStatusEnum;
    questionType?: QuestionTypeEnum;
}
export interface QuestionResponse {
    id: string;
    userId: string;
    courseId: string;
    questionText : string;
    questionType: QuestionTypeEnum;
    points: number;
    status: QuestionStatusEnum;
    createdAt: string;
    updatedAt: string;
}
// Question Choice related interfaces 
export interface QuestionChoiceCreateRequest {
    questionId: string;
    choiceText: string;
    isCorrect: boolean;
}
export interface QuestionChoiceUpdateRequest {
    choiceText?: string | null;
    isCorrect?: boolean | null;
}
export interface QuestionChoiceResponse {
    id: string;
    questionId: string;
    choiceText: string;
    isCorrect: boolean;
    createdAt: string;
    updatedAt: string;
}
// Question Answer related interfaces
export interface QuestionAnswerCreateRequest {
    questionId: string;
    answerText: string;
}
export interface QuestionAnswerUpdateRequest {
    answerText?: string | null;
}
export interface QuestionAnswerResponse {
    id: string;
    questionId: string;
    answerText: string;
    createdAt: string;
    updatedAt: string;
}
// Quiz Question related interfaces
export interface QuizQuestionCreateRequest {
    quizId: string;
    questionId: string;
    questionOrder?: number | null;
    overridePoints?: number | null;
}
export interface QuizQuestionUpdateRequest {
    questionOrder?: number | null;
    overridePoints?: number | null;
}
export interface QuizQuestionResponse {
    id: string;
    quizId: string;
    questionId: string;
    questionOrder?: number | null;
    overridePoints?: number | null;
}

export const quizAPI = {
    // Create a new quiz
    async createQuiz(quiz: QuizCreateRequest): Promise<QuizResponse> {
        const response = await lessonAxios.post<QuizResponse>(API_ENDPOINTS.QUIZZES.BASE, quiz);
        return response.data;
    },
    // Update an existing quiz
    async updateQuiz(id: string, quiz: Partial<QuizUpdateRequest>): Promise<QuizResponse> {
        const response = await lessonAxios.put<QuizResponse>(`${API_ENDPOINTS.QUIZZES.BASE}/${id}`, quiz);
        return response.data;
    },
    // Get quiz by ID
    async getQuizById(id: string): Promise<QuizResponse> {
        const response = await lessonAxios.get<QuizResponse>(`${API_ENDPOINTS.QUIZZES.BASE}/${id}`);
        return response.data;
    },
    // Get all quizzes
    async getAllQuizzes(): Promise<QuizResponse[]> {
        const response = await lessonAxios.get<QuizResponse[]>(API_ENDPOINTS.QUIZZES.BASE);
        return response.data;
    },

    // Delete a quiz
    async deleteQuiz(id: string): Promise<void> {
        await lessonAxios.delete<void>(`${API_ENDPOINTS.QUIZZES.BASE}/${id}`);
    },
    //Disable a quiz
    async disableQuiz(id: string): Promise<void> {
        await lessonAxios.patch<void>(`${API_ENDPOINTS.QUIZZES.BASE}/disable/${id}`);
    }
};
export const questionAPI = {
    // Create a new question
    async createQuestion(question: QuestionCreateRequest): Promise<QuestionResponse> {
        const response = await lessonAxios.post<QuestionResponse>(`${API_ENDPOINTS.QUIZZES.BASE}/questions`, question);
        return response.data;
    },
    // Update an existing question
    async updateQuestion(id: string, question: Partial<QuestionUpdateRequest>): Promise<QuestionResponse> { 
        const response = await lessonAxios.put<QuestionResponse>(`${API_ENDPOINTS.QUIZZES.BASE}/questions/${id}`, question);
        return response.data;
    },
    // Get question by ID
    async getQuestionById(id: string): Promise<QuestionResponse> {
        const response = await lessonAxios.get<QuestionResponse>(`${API_ENDPOINTS.QUIZZES.BASE}/questions/${id}`);
        return response.data;
    },
    // Get all questions
    async getAllQuestions(): Promise<QuestionResponse[]> {   
        const response = await lessonAxios.get<QuestionResponse[]>(`${API_ENDPOINTS.QUIZZES.BASE}/questions`);
        return response.data;
    },
    // Delete a question
    async deleteQuestion(id: string): Promise<void> {
        await lessonAxios.delete<void>(`${API_ENDPOINTS.QUIZZES.BASE}/questions/${id}`);
    },
    //Disable a question
    async disableQuestion(id: string): Promise<void> {
        await lessonAxios.patch<void>(`${API_ENDPOINTS.QUIZZES.BASE}/questions/disable/${id}`);
    },
};
export const questionChoiceAPI = {
    // Create a new question choice
    async createQuestionChoice(choice: QuestionChoiceCreateRequest): Promise<QuestionChoiceResponse> {
        const response = await lessonAxios.post<QuestionChoiceResponse>(`${API_ENDPOINTS.QUIZZES.BASE}/question-choices`, choice);
        return response.data;
    },
    // Update an existing question choice
    async updateQuestionChoice(id: string, choice: Partial<QuestionChoiceUpdateRequest>): Promise<QuestionChoiceResponse> {
        const response = await lessonAxios.put<QuestionChoiceResponse>(`${API_ENDPOINTS.QUIZZES.BASE}/question-choices/${id}`, choice);
        return response.data;
    },
    // Get question choice by ID
    async getQuestionChoiceById(id: string): Promise<QuestionChoiceResponse> {
        const response = await lessonAxios.get<QuestionChoiceResponse>(`${API_ENDPOINTS.QUIZZES.BASE}/question-choices/${id}`);
        return response.data;
    },
    // Get all question choices
    async getAllQuestionChoices(): Promise<QuestionChoiceResponse[]> {
        const response = await lessonAxios.get<QuestionChoiceResponse[]>(`${API_ENDPOINTS.QUIZZES.BASE}/question-choices`);
        return response.data;
    },
    // Delete a question choice
    async deleteQuestionChoice(id: string): Promise<void> {
        await lessonAxios.delete<void>(`${API_ENDPOINTS.QUIZZES.BASE}/question-choices/${id}`);
    },
};
const questionAnswerAPI = {
    // Create a new question answer
    async createQuestionAnswer(answer: QuestionAnswerCreateRequest): Promise<QuestionAnswerResponse> {
        const response = await lessonAxios.post<QuestionAnswerResponse>(`${API_ENDPOINTS.QUIZZES.BASE}/question-answers`, answer);
        return response.data;
    },  
    // Update an existing question answer
    async updateQuestionAnswer(id: string, answer: Partial<QuestionAnswerUpdateRequest>): Promise<QuestionAnswerResponse> {
        const response = await lessonAxios.put<QuestionAnswerResponse>(`${API_ENDPOINTS.QUIZZES.BASE}/question-answers/${id}`, answer);
        return response.data;
    },
    // Get question answer by ID
    async getQuestionAnswerById(id: string): Promise<QuestionAnswerResponse> {
        const response = await lessonAxios.get<QuestionAnswerResponse>(`${API_ENDPOINTS.QUIZZES.BASE}/question-answers/${id}`);
        return response.data;
    },
    // Get all question answers
    async getAllQuestionAnswers(): Promise<QuestionAnswerResponse[]> {
        const response = await lessonAxios.get<QuestionAnswerResponse[]>(`${API_ENDPOINTS.QUIZZES.BASE}/question-answers`);
        return response.data;
    },
    // Delete a question answer
    async deleteQuestionAnswer(id: string): Promise<void> {
        await lessonAxios.delete<void>(`${API_ENDPOINTS.QUIZZES.BASE}/question-answers/${id}`);
    },
};
export const quizQuestionAPI = {
    // Create a new quiz question
    async createQuizQuestion(quizQuestion: QuizQuestionCreateRequest): Promise<QuizQuestionResponse> {
        const response = await lessonAxios.post<QuizQuestionResponse>(`${API_ENDPOINTS.QUIZZES.BASE}/quiz-questions`, quizQuestion);
        return response.data;
    },
    // Update an existing quiz question
    async updateQuizQuestion(id: string, quizQuestion: Partial<QuizQuestionUpdateRequest>): Promise<QuizQuestionResponse> { 
        const response = await lessonAxios.put<QuizQuestionResponse>(`${API_ENDPOINTS.QUIZZES.BASE}/quiz-questions/${id}`, quizQuestion);
        return response.data;
    },
    // Get quiz question by ID
    async getQuizQuestionById(id: string): Promise<QuizQuestionResponse> {
        const response = await lessonAxios.get<QuizQuestionResponse>(`${API_ENDPOINTS.QUIZZES.BASE}/quiz-questions/${id}`);
        return response.data;
    },
    // Get all quiz questions
    async getAllQuizQuestions(): Promise<QuizQuestionResponse[]> {
        const response = await lessonAxios.get<QuizQuestionResponse[]>(`${API_ENDPOINTS.QUIZZES.BASE}/quiz-questions`);
        return response.data;
    },
    // Delete a quiz question
    async deleteQuizQuestion(id: string): Promise<void> {
        await lessonAxios.delete<void>(`${API_ENDPOINTS.QUIZZES.BASE}/quiz-questions/${id}`);
    },
};
