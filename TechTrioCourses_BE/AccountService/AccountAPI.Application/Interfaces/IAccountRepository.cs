
using AccountAPI.Domain.Entities;

namespace AccountAPI.Application.Interfaces
{
    public interface IAccountRepository
    {
        Task<Account?> GetByEmailAsync(string email);
        Task<Account?> GetByIdAsync(Guid id);
        Task<Account> CreateAccountAsync(Account account);
       
        Task<bool> UpdateAccountAsync(Account account);
        Task<bool> EmailExistsAsync(string email);
    }
}
