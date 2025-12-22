import { API_ENDPOINTS } from '@/constants/apiURL';
import { courseAxios } from '@/middleware/axiosMiddleware';

export interface CourseResponse {
  id: string;
  title: string;
  description: string | null;
  categoryId: string | null;
  creatorId: string | null;
  status: CourseStatusEnum;
  createdAt: string | null;
  updatedAt: string | null;
  categoryName: string | null;
  creatorName: string | null;
  totalLessons: number;
  totalQuizzes: number;
  averageRating: number;
}
export enum CourseStatusEnum {
  Hidden = 1,
  Published = 2

}
export interface CourseCreateRequest {
  title: string;
  description?: string | null;
  categoryId?: string | null;
  creatorId?: string | null;
  status:CourseStatusEnum;
}
export interface CourseUpdateRequest {
  title?: string;
  description?: string | null;
  categoryId?: string | null;
  status:CourseStatusEnum;
}

export const courseAPI = {
  // Lấy danh sách tất cả courses
  async getAllCourses(): Promise<CourseResponse[]> {
    const response = await courseAxios.get<CourseResponse[]>(API_ENDPOINTS.COURSES.BASE);
    return response.data;
  },

  // Lấy chi tiết một course theo ID
  async getCourseById(id: string): Promise<CourseResponse> {
    const response = await courseAxios.get<CourseResponse>(`${API_ENDPOINTS.COURSES.BASE}/${id}`);
    return response.data;
  },

  // Tạo course mới
  async createCourse(course: CourseCreateRequest): Promise<CourseResponse> {
    const response = await courseAxios.post<CourseResponse>(API_ENDPOINTS.COURSES.BASE, course);
    return response.data;
  },

  // Cập nhật course
  async updateCourse(id: string, course: CourseUpdateRequest): Promise<CourseResponse> {
    const response = await courseAxios.put<CourseResponse>(`${API_ENDPOINTS.COURSES.BASE}/${id}`, course);
    return response.data;
  },

  // Xóa course (hard delete)
  async deleteCourse(id: string): Promise<void> {
    await courseAxios.delete(`${API_ENDPOINTS.COURSES.BASE}/${id}`);
  },

  // Disable course (soft delete)
  async disableCourse(id: string): Promise<void> {
    await courseAxios.put(`${API_ENDPOINTS.COURSES.BASE}/${id}/disable`);
  },
};
