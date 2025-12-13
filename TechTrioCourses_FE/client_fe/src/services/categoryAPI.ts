import { API_ENDPOINTS } from '@/constants/apiURL';
import { categoryAxios } from '@/middleware/axiosMiddleware';
export interface CategoryResponse {
  id: string;
  name: string;
  description: string | null;
  
}
export interface CategoryCreateRequest {
    name: string;
    description?: string | null;
}
export interface CategoryUpdateRequest {
    name: string;
    description?: string | null;
}
export const categoryAPI = {
  // Lấy danh sách tất cả categories
  async getAllCategories(): Promise<CategoryResponse[]> {
    const response = await categoryAxios.get<CategoryResponse[]>(API_ENDPOINTS.CATEGORIES.BASE);
    return response.data;
  },
    // Lấy chi tiết một category theo ID
    async getCategoryById(id: string): Promise<CategoryResponse> {
    const response = await categoryAxios.get<CategoryResponse>(`${API_ENDPOINTS.CATEGORIES.BASE}/${id}`);
    return response.data;
    },
    // Tạo category mới
    async createCategory(category: Partial<CategoryCreateRequest>): Promise<CategoryResponse> {
    const response = await categoryAxios.post<CategoryResponse>(API_ENDPOINTS.CATEGORIES.BASE, category);
    return response.data;
    }
    ,
    // Cập nhật category
    async updateCategory(id: string, category: Partial<CategoryUpdateRequest>): Promise<CategoryResponse> {
    const response = await categoryAxios.put<CategoryResponse>(`${API_ENDPOINTS.CATEGORIES.BASE}/${id}`, category);
    return response.data;
    },
    // Xóa category
    async deleteCategory(id: string): Promise<void> {
    await categoryAxios.delete<void>(`${API_ENDPOINTS.CATEGORIES.BASE}/${id}`);
  }
}
