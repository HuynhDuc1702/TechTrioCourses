import { API_ENDPOINTS } from '@/constants/apiURL';
import { quizAxios } from '@/middleware/axiosMiddleware';
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
    questionText: string;
    questionType: QuestionTypeEnum;
    points: number;
    status: QuestionStatusEnum;
}
export interface QuestionUpdateRequest {
    questionText?: string | null;
    points?: number | null;
    status?: QuestionStatusEnum;
    questionType?: QuestionTypeEnum;
}
export interface QuestionResponse {
    id: string;
    userId: string;
    courseId: string;
    questionText: string;
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

    quizId: string;
    questionId: string;
    questionOrder?: number | null;
    overridePoints?: number | null;
}

export const quizAPI = {
    // Create a new quiz
    async createQuiz(quiz: QuizCreateRequest): Promise<QuizResponse> {
        const response = await quizAxios.post<QuizResponse>(API_ENDPOINTS.QUIZZES.BASE, quiz);
        return response.data;
    },
    // Update an existing quiz
    async updateQuiz(id: string, quiz: Partial<QuizUpdateRequest>): Promise<QuizResponse> {
        const response = await quizAxios.put<QuizResponse>(`${API_ENDPOINTS.QUIZZES.BASE}/${id}`, quiz);
        return response.data;
    },
    // Get quiz by ID
    async getQuizById(id: string): Promise<QuizResponse> {
        const response = await quizAxios.get<QuizResponse>(`${API_ENDPOINTS.QUIZZES.BASE}/${id}`);
        return response.data;
    },
    // Get all quizzes
    async getAllQuizzes(): Promise<QuizResponse[]> {
        const response = await quizAxios.get<QuizResponse[]>(API_ENDPOINTS.QUIZZES.BASE);
        return response.data;
    },
    // Get quizzes by Course ID
    async getQuizzesByCourseId(courseId: string): Promise<QuizResponse[]> {
        const response = await quizAxios.get<QuizResponse[]>(`${API_ENDPOINTS.QUIZZES.BASE}/course/${courseId}`);
        return response.data;
    },
    // Delete a quiz
    async deleteQuiz(id: string): Promise<void> {
        await quizAxios.delete<void>(`${API_ENDPOINTS.QUIZZES.BASE}/${id}`);
    },
    //Disable a quiz
    async disableQuiz(id: string): Promise<void> {
        await quizAxios.put<void>(`${API_ENDPOINTS.QUIZZES.BASE}/${id}/disable`);
    },

};
export const questionAPI = {
    // Create a new question
    async createQuestion(question: QuestionCreateRequest): Promise<QuestionResponse> {
        const response = await quizAxios.post<QuestionResponse>(API_ENDPOINTS.QUESTIONS.BASE, question);
        return response.data;
    },
    // Update an existing question
    async updateQuestion(id: string, question: Partial<QuestionUpdateRequest>): Promise<QuestionResponse> {
        const response = await quizAxios.put<QuestionResponse>(`${API_ENDPOINTS.QUESTIONS.BASE}/${id}`, question);
        return response.data;
    },
    // Get question by ID
    async getQuestionById(id: string): Promise<QuestionResponse> {
        const response = await quizAxios.get<QuestionResponse>(`${API_ENDPOINTS.QUESTIONS.BASE}/${id}`);
        return response.data;
    },
    // Get all questions
    async getAllQuestions(): Promise<QuestionResponse[]> {
        const response = await quizAxios.get<QuestionResponse[]>(API_ENDPOINTS.QUESTIONS.BASE);
        return response.data;
    },
    // Get questions by Course ID
    async getQuestionsByCourseId(courseId: string): Promise<QuestionResponse[]> {
        const response = await quizAxios.get<QuestionResponse[]>(`${API_ENDPOINTS.QUESTIONS.BASE}/course/${courseId}`);
        return response.data;
    },
    // Delete a question
    async deleteQuestion(id: string): Promise<void> {
        await quizAxios.delete<void>(`${API_ENDPOINTS.QUESTIONS.BASE}/${id}`);
    },
    //Disable a question
    async disableQuestion(id: string): Promise<void> {
        await quizAxios.put<void>(`${API_ENDPOINTS.QUESTIONS.BASE}/${id}/disable`);
    },

};
export const questionChoiceAPI = {
    // Create a new question choice
    async createQuestionChoice(choice: QuestionChoiceCreateRequest): Promise<QuestionChoiceResponse> {
        const response = await quizAxios.post<QuestionChoiceResponse>(API_ENDPOINTS.QUESTION_CHOICES.BASE, choice);
        return response.data;
    },
    // Update an existing question choice
    async updateQuestionChoice(id: string, choice: Partial<QuestionChoiceUpdateRequest>): Promise<QuestionChoiceResponse> {
        const response = await quizAxios.put<QuestionChoiceResponse>(`${API_ENDPOINTS.QUESTION_CHOICES.BASE}/${id}`, choice);
        return response.data;
    },
    // Get question choice by ID
    async getQuestionChoiceById(id: string): Promise<QuestionChoiceResponse> {
        const response = await quizAxios.get<QuestionChoiceResponse>(`${API_ENDPOINTS.QUESTION_CHOICES.BASE}/${id}`);
        return response.data;
    },
    // Get question choices by Question ID
    async getQuestionChoicesByQuestionId(questionId: string): Promise<QuestionChoiceResponse[]> {
        const response = await quizAxios.get<QuestionChoiceResponse[]>(`${API_ENDPOINTS.QUESTION_CHOICES.BASE}/question/${questionId}`);
        return response.data;
    },
    // Get all question choices
    async getAllQuestionChoices(): Promise<QuestionChoiceResponse[]> {
        const response = await quizAxios.get<QuestionChoiceResponse[]>(API_ENDPOINTS.QUESTION_CHOICES.BASE);
        return response.data;
    },
    // Delete a question choice
    async deleteQuestionChoice(id: string): Promise<void> {
        await quizAxios.delete<void>(`${API_ENDPOINTS.QUESTION_CHOICES.BASE}/${id}`);
    },
};
export const questionAnswerAPI = {
    // Create a new question answer
    async createQuestionAnswer(answer: QuestionAnswerCreateRequest): Promise<QuestionAnswerResponse> {
        const response = await quizAxios.post<QuestionAnswerResponse>(API_ENDPOINTS.QUESTION_ANSWERS.BASE, answer);
        return response.data;
    },
    // Update an existing question answer
    async updateQuestionAnswer(id: string, answer: Partial<QuestionAnswerUpdateRequest>): Promise<QuestionAnswerResponse> {
        const response = await quizAxios.put<QuestionAnswerResponse>(`${API_ENDPOINTS.QUESTION_ANSWERS.BASE}/${id}`, answer);
        return response.data;
    },
    // Get question answer by ID
    async getQuestionAnswerById(id: string): Promise<QuestionAnswerResponse> {
        const response = await quizAxios.get<QuestionAnswerResponse>(`${API_ENDPOINTS.QUESTION_ANSWERS.BASE}/${id}`);
        return response.data;
    },
    // Get question answers by Question ID
    async getQuestionAnswersByQuestionId(questionId: string): Promise<QuestionAnswerResponse[]> {
        const response = await quizAxios.get<QuestionAnswerResponse[]>(`${API_ENDPOINTS.QUESTION_ANSWERS.BASE}/question/${questionId}`);
        return response.data;
    },
    // Get all question answers
    async getAllQuestionAnswers(): Promise<QuestionAnswerResponse[]> {
        const response = await quizAxios.get<QuestionAnswerResponse[]>(API_ENDPOINTS.QUESTION_ANSWERS.BASE);
        return response.data;
    },
    // Delete a question answer
    async deleteQuestionAnswer(id: string): Promise<void> {
        await quizAxios.delete<void>(`${API_ENDPOINTS.QUESTION_ANSWERS.BASE}/${id}`);
    },
};
export const quizQuestionAPI = {
    // Create a new quiz question
    async createQuizQuestion(quizQuestion: QuizQuestionCreateRequest): Promise<QuizQuestionResponse> {
        const response = await quizAxios.post<QuizQuestionResponse>(API_ENDPOINTS.QUIZ_QUESTIONS.BASE, quizQuestion);
        return response.data;
    },
    // Update an existing quiz question
    async updateQuizQuestion(quizId: string, questionId: string, quizQuestion: Partial<QuizQuestionUpdateRequest>): Promise<QuizQuestionResponse> {
        const response = await quizAxios.put<QuizQuestionResponse>(`${API_ENDPOINTS.QUIZ_QUESTIONS.BASE}/${quizId}/${questionId}`, quizQuestion);
        return response.data;
    },
    // Get quiz question by ID
    async getQuizQuestionById(quizId: string, questionId: string): Promise<QuizQuestionResponse> {
        const response = await quizAxios.get<QuizQuestionResponse>(`${API_ENDPOINTS.QUIZ_QUESTIONS.BASE}/${quizId}/${questionId}`);
        return response.data;
    },
    //get quiz questions by quiz ID
    async getQuizQuestionsByQuizId(quizId: string): Promise<QuizQuestionResponse[]> {
        const response = await quizAxios.get<QuizQuestionResponse[]>(`${API_ENDPOINTS.QUIZ_QUESTIONS.BASE}/quiz/${quizId}`);
        return response.data;
    },
    //get quiz questions by question ID
    async getQuizQuestionsByQuestionId(questionId: string): Promise<QuizQuestionResponse[]> {
        const response = await quizAxios.get<QuizQuestionResponse[]>(`${API_ENDPOINTS.QUIZ_QUESTIONS.BASE}/question/${questionId}`);
        return response.data;
    },
    // Get all quiz questions
    async getAllQuizQuestions(): Promise<QuizQuestionResponse[]> {
        const response = await quizAxios.get<QuizQuestionResponse[]>(API_ENDPOINTS.QUIZ_QUESTIONS.BASE);
        return response.data;
    },
    // Delete a quiz question
    async deleteQuizQuestion(quizId: string, questionId: string): Promise<void> {
        await quizAxios.delete<void>(`${API_ENDPOINTS.QUIZ_QUESTIONS.BASE}/${quizId}/${questionId}`);
    },
};
