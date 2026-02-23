using TechTrioCourses.Shared.Repositories.Interfaces;
using UserAPI.Domain.Entities;

namespace UserAPI.Application.Interfaces.IRepositories
{
    public interface IUserRepository :IGenericRepository<User>
    {
    
        Task<User?> GetByAccountIdAsync(Guid accountId);
    
    }
}
