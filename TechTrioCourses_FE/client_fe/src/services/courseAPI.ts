import { API_ENDPOINTS } from '@/constants/apiURL';
import { courseAxios } from '@/middleware/axiosMiddleware';

export interface Course {
  id: string;
  title: string;
  description: string | null;
  categoryId: string | null;
  creatorId: string | null;
  status: number;
  createdAt: string | null;
  updatedAt: string | null;
  categoryName: string | null;
  creatorName: string | null;
  totalLessons: number;
  totalQuizzes: number;
  averageRating: number;
}

export const courseAPI = {
  // Lấy danh sách tất cả courses
  async getAllCourses(): Promise<Course[]> {
    const response = await courseAxios.get<Course[]>(API_ENDPOINTS.COURSES.BASE);
    return response.data;
  },

  // Lấy chi tiết một course theo ID
  async getCourseById(id: string): Promise<Course> {
    const response = await courseAxios.get<Course>(`${API_ENDPOINTS.COURSES.BASE}/${id}`);
    return response.data;
  },

  // Tạo course mới
  async createCourse(course: Partial<Course>): Promise<Course> {
    const response = await courseAxios.post<Course>(API_ENDPOINTS.COURSES.BASE, course);
    return response.data;
  },

  // Cập nhật course
  async updateCourse(id: string, course: Partial<Course>): Promise<Course> {
    const response = await courseAxios.put<Course>(`${API_ENDPOINTS.COURSES.BASE}/${id}`, course);
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
