using AccountAPI.Models;

namespace AccountAPI.Repositories.Interfaces
{
    public interface IAccountRepo
    {
        Task<Account?> GetByEmailAsync(string email);
        Task<Account?> GetByIdAsync(Guid id);
        Task<Account> CreateAccountAsync(Account account);
       
        Task<bool> UpdateAccountAsync(Account account);
        Task<bool> EmailExistsAsync(string email);
    }
}
