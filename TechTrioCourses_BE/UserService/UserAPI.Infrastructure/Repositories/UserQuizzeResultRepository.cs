using Microsoft.EntityFrameworkCore;
using TechTrioCourses.Shared.Enums;
using UserAPI.Infrastructure.Data;
using UserAPI.Domain.Entities;
using UserAPI.Application.Interfaces.IRepositories;
using TechTrioCourses.Shared.Repositories;

namespace UserAPI.Infrastructure.Repositories
{
    public class UserQuizzeResultRepository : GenericRepository<UserQuizzeResult, UserDbContext>,IUserQuizzeResultRepository
    {

        public UserQuizzeResultRepository(UserDbContext context): base(context)
        {
        
        
        }
        protected override DbSet<UserQuizzeResult> DbSet => _context.UserQuizzeResults;

       

        public async Task<IEnumerable<UserQuizzeResult>> GetByUserIdAsync(Guid userId)
        {
            return await _context.UserQuizzeResults
            .Where(qr => qr.UserId == userId)
            .OrderByDescending(qr => qr.AttemptNumber)
            .ToListAsync();
        }

        public async Task<IEnumerable<UserQuizzeResult>> GetByQuizIdAsync(Guid quizId)
        {
            return await _context.UserQuizzeResults
            .Where(qr => qr.QuizId == quizId)
           .OrderByDescending(qr => qr.AttemptNumber)
                .ToListAsync();
        }

        public async Task<IEnumerable<UserQuizzeResult>> GetByUserAndQuizIdAsync(Guid userId, Guid quizId)
        {
            return await _context.UserQuizzeResults
            .Where(qr => qr.UserId == userId && qr.QuizId == quizId)
                  .OrderByDescending(qr => qr.AttemptNumber)
                   .ToListAsync();
        }

        public async Task<IEnumerable<UserQuizzeResult>> GetByUserQuizIdAsync(Guid userQuizId)
        {
            return await _context.UserQuizzeResults
     .Where(qr => qr.UserQuizId == userQuizId)
             .OrderByDescending(qr => qr.AttemptNumber)
         .ToListAsync();
        }
        public async Task<UserQuizzeResult?> GetLatestByUserQuizIdAsync(Guid userQuizId)
        {
            return await _context.UserQuizzeResults
                .Where(qr => qr.UserQuizId == userQuizId)
                .OrderByDescending(qr => qr.AttemptNumber)
                .FirstOrDefaultAsync();
        }


        
  
    }
}
