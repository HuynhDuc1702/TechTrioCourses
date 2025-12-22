import { API_ENDPOINTS } from '@/constants/apiURL';
import { lessonAxios } from '@/middleware/axiosMiddleware';

export interface LessonResponse {
  id: string;

  courseId: string;
  courseName?: string | null;

  title: string;
  content?: string | null;

  mediaUrl?: string | null;
  mediaType?: string | null;

  orderIndex?: number | null;

  status: LessonStatusEnum;

  createdAt: string;
  updatedAt: string;
}
export enum LessonStatusEnum {
  Hidden = 1,
  Published = 2
 
}
export interface LessonCreateRequest {
  courseId: string;
  title: string;
  content?: string | null;
  mediaUrl?: string | null;
  mediaType?: string | null;
  orderIndex?: number | null;
  status:LessonStatusEnum;
}
export interface LessonUpdateRequest {
    courseId: string;
  title?: string;
  content?: string | null;
  mediaUrl?: string | null;
  mediaType?: string | null;
  orderIndex?: number | null;
  status:LessonStatusEnum;
}
export const lessonAPI = {
  // Get all lessons
  async getAllLessons(): Promise<LessonResponse[]> {
    const response = await lessonAxios.get<LessonResponse[]>(API_ENDPOINTS.LESSONS.BASE);
    return response.data;
  },
   // Get lessons by course ID
    async getLessonsByCourseId(courseId: string): Promise<LessonResponse[]> {
    const response = await lessonAxios.get<LessonResponse[]>(`${API_ENDPOINTS.LESSONS.BASE}/course/${courseId}`);
    return response.data;
    },
    // Get lesson details by ID 
    async getLessonById(id: string): Promise<LessonResponse> {
    const response = await lessonAxios.get<LessonResponse>(`${API_ENDPOINTS.LESSONS.BASE}/${id}`);
    return response.data;
    }
    ,
    // Create new lesson
    async createLesson(lesson: Partial<LessonCreateRequest>): Promise<LessonResponse> {
    const response = await lessonAxios.post<LessonResponse>(API_ENDPOINTS.LESSONS.BASE, lesson);
    return response.data;
    }
    ,
    // Update lesson
    async updateLesson(id: string, lesson: Partial<LessonUpdateRequest>): Promise<LessonResponse> {
    const response = await lessonAxios.put<LessonResponse>(`${API_ENDPOINTS.LESSONS.BASE}/${id}`, lesson);
    return response.data;
    }
    ,
    // delete lesson
    async deleteLesson(id: string): Promise<void> {
    await lessonAxios.delete<void>(`${API_ENDPOINTS.LESSONS.BASE}/${id}`);
  },
  //Disable lesson
  async disableLesson(id: string): Promise<void> {
    await lessonAxios.patch<void>(`${API_ENDPOINTS.LESSONS.BASE}/disable/${id}`);
  }
}