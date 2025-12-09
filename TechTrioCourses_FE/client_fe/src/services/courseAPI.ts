import { API_URLS, API_ENDPOINTS } from '@/constants/apiURL';
const API_BASE_URL = API_URLS.COURSE;

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
    const response = await fetch(`${API_BASE_URL}${API_ENDPOINTS.COURSES.BASE}`);
    if (!response.ok) {
      throw new Error(`Error: ${response.statusText}`);
    }
    return response.json();
  },

  // Lấy chi tiết một course theo ID
  async getCourseById(id: string): Promise<Course> {
    const response = await fetch(`${API_BASE_URL}${API_ENDPOINTS.COURSES.BASE}/${id}`);
    if (!response.ok) {
      throw new Error(`Error: ${response.statusText}`);
    }
    return response.json();
  },

  // Tạo course mới
  async createCourse(course: Partial<Course>): Promise<Course> {
    const response = await fetch(`${API_BASE_URL}${API_ENDPOINTS.COURSES.BASE}`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(course),
    });
    if (!response.ok) {
      throw new Error(`Error: ${response.statusText}`);
    }
    return response.json();
  },

  // Cập nhật course
  async updateCourse(id: string, course: Partial<Course>): Promise<Course> {
    const response = await fetch(`${API_BASE_URL}${API_ENDPOINTS.COURSES.BASE}/${id}`, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(course),
    });
    if (!response.ok) {
      throw new Error(`Error: ${response.statusText}`);
    }
    return response.json();
  },

  // Xóa course
  async deleteCourse(id: string): Promise<void> {
    const response = await fetch(`${API_BASE_URL}${API_ENDPOINTS.COURSES.BASE}/${id}`, {
      method: "DELETE",
    });
    if (!response.ok) {
      throw new Error(`Error: ${response.statusText}`);
    }
  },
};
