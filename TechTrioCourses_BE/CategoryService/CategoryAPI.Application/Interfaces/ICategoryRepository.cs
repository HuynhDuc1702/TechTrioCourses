using CategoryAPI.Domain.Entities;

namespace CategoryAPI.Application.Interfaces
{
    public interface ICategoryRepository
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
