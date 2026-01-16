using CategoryAPI.DTOs.Request;
using CategoryAPI.DTOs.Response;

namespace CategoryAPI.Services.Interfaces
{
    public interface ICategoryService
    {
         Task<IEnumerable<CategoryResponse>> GetAllCategorysAsync();
        Task<CategoryResponse?> GetCategoryByIdAsync(Guid id);
        Task<IEnumerable<CategoryResponse>> GetCategoriesByIdsAsync(List<Guid> ids);
        Task<CategoryResponse> CreateCategoryAsync(CreateCategoryRequest request);
        Task<CategoryResponse?> UpdateCategoryAsync(Guid id, UpdateCategoryRequest request);
        Task<bool> DeleteCategoryAsync(Guid id);
    }
}
