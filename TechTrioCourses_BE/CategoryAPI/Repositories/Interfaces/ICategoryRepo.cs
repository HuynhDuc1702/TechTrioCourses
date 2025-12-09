using CategoryAPI.Models;

namespace CategoryAPI.Repositories.Interfaces
{
    public interface ICategoryRepo
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(Guid id);
        Task<IEnumerable<Category>> GetByIdsAsync(List<Guid> ids);
        Task<Category> CreateAsync(Category Category);
        Task<Category?> UpdateAsync(Category Category);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
    }
}
