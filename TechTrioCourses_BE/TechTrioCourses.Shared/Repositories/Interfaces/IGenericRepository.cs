
namespace TechTrioCourses.Shared.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T: class

    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetByIdsAsync(List<Guid> ids);
        Task<T> CreateAsync(T entity);
        Task<T?> UpdateAsync(T entity);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
    }
}
