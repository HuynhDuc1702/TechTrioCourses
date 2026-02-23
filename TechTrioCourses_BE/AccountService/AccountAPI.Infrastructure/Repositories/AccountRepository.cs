using Microsoft.EntityFrameworkCore;
using AccountAPI.Domain.Entities;
using AccountAPI.Application.Interfaces;
using AccountAPI.Infrastructure.Data;
using TechTrioCourses.Shared.Repositories;

namespace AccountAPI.Infrastructure.Repositories
{
    public class AccountRepository : GenericRepository<Account,AccountDbContext>,IAccountRepository
    {
      
        public AccountRepository(AccountDbContext context) :base(context) 
        {
         
        }
        protected override DbSet<Account> DbSet => _context.Accounts;

        public async Task<Account?> GetByEmailAsync(string email)
        {
            return await _context.Accounts
                .FirstOrDefaultAsync(a => a.Email == email);
        }

      
        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Accounts.AnyAsync(a => a.Email == email);
        }
    }
}
