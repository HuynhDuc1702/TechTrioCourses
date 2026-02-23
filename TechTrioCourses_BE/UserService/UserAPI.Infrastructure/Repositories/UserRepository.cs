using Microsoft.EntityFrameworkCore;
using UserAPI.Infrastructure.Data;
using UserAPI.Domain.Entities;
using UserAPI.Application.Interfaces.IRepositories;
using TechTrioCourses.Shared.Repositories;

namespace UserAPI.Infrastructure.Repositories
{
    public class UserRepository : GenericRepository<User, UserDbContext>,IUserRepository
    {
     

        public UserRepository(UserDbContext context):base(context) 
        {
        
        }
        protected override DbSet<User> DbSet => _context.Users;

      
        public async Task<User?> GetByAccountIdAsync(Guid accountId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.AccountId == accountId);
        }

      
       

      
    }
}
