using AccountAPI.Repositories.Interfaces;
using AccountAPI.Datas;
using AccountAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AccountAPI.Repositories
{
    public class AccountRepo : IAccountRepo
    {
        private readonly AccountContext _context;

        public AccountRepo(AccountContext context)
        {
            _context = context;
        }

        public async Task<Account?> GetByEmailAsync(string email)
        {
            return await _context.Accounts
                
                .FirstOrDefaultAsync(a => a.Email == email);
        }

        public async Task<Account?> GetByIdAsync(Guid id)
        {
            return await _context.Accounts
              
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Account> CreateAccountAsync(Account account)
        {
            account.Id = Guid.NewGuid();
            account.CreatedAt = DateTime.UtcNow;
            account.UpdatedAt = DateTime.UtcNow;

            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

            return account;
        }


        public async Task<bool> UpdateAccountAsync(Account account)
        {
            account.UpdatedAt = DateTime.UtcNow;
            _context.Accounts.Update(account);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Accounts.AnyAsync(a => a.Email == email);
        }
    }
}
