using Microsoft.EntityFrameworkCore;
using UserAPI.Datas;
using UserAPI.Models;
using UserAPI.Repositories.Interfaces;

namespace UserAPI.Repositories
{
    public class UserRepo : IUserRepo
    {
        private readonly TechTrioUsersContext _context;

        public UserRepo(TechTrioUsersContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }
        public async Task<IEnumerable<User>> GetAllAsync() 
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> GetByAccountIdAsync(Guid accountId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.AccountId == accountId);
        }

        public async Task<IEnumerable<User>> GetByIdsAsync(List<Guid> ids)
        {
            return await _context.Users
   .Where(u => ids.Contains(u.Id))
           .ToListAsync();
        }

        public async Task<User> CreateUserAsync(User user)
        {
            user.Id = Guid.NewGuid();
            user.CreatedAt = DateTime.UtcNow;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UserExistsAsync(Guid accountId)
        {
            return await _context.Users.AnyAsync(u => u.AccountId == accountId);
        }
    }
}
