using UserAPI.Models;

namespace UserAPI.Repositories.Interfaces
{
    public interface IUserRepo
    {
        Task<User?> GetByIdAsync(Guid id);
        Task<User?> GetByAccountIdAsync(Guid accountId);
        Task<IEnumerable<User>> GetByIdsAsync(List<Guid> ids);
        Task<User> CreateUserAsync(User user);
        Task<bool> UpdateUserAsync(User user);
        Task<bool> UserExistsAsync(Guid accountId);
        Task<IEnumerable<User>> GetAllAsync();
    }
}
