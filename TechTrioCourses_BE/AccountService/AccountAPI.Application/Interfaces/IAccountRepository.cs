
using AccountAPI.Domain.Entities;
using TechTrioCourses.Shared.Repositories.Interfaces;

namespace AccountAPI.Application.Interfaces
{
    public interface IAccountRepository : IGenericRepository<Account>
    {
        Task<Account?> GetByEmailAsync(string email);
    
  
       
     
        Task<bool> EmailExistsAsync(string email);
    }
}
