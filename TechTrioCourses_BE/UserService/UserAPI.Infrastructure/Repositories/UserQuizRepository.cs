using Microsoft.EntityFrameworkCore;

using UserAPI.Infrastructure.Data;
using UserAPI.Domain.Entities;
using UserAPI.Application.Interfaces.IRepositories;
using TechTrioCourses.Shared.Repositories;

namespace UserAPI.Infrastructure.Repositories
{
    public class UserQuizRepository : GenericRepository<UserQuiz, UserDbContext>,IUserQuizRepository
    {
     
        public UserQuizRepository(UserDbContext context): base(context) 
        {
         
        }
        protected override DbSet<UserQuiz> DbSet => _context.UserQuizzes;

      

        public async Task<IEnumerable<UserQuiz>> GetByUserIdAsync(Guid userId)
        {
            return await _context.Set<UserQuiz>()
             .Where(uq => uq.UserId == userId)
                  .ToListAsync();
        }

        public async Task<IEnumerable<UserQuiz>> GetByQuizIdAsync(Guid quizId)
        {
            return await _context.Set<UserQuiz>()
           .Where(uq => uq.QuizId == quizId)
       .ToListAsync();
        }

        public async Task<IEnumerable<UserQuiz>> GetByCourseIdAsync(Guid courseId)
        {
            return await _context.Set<UserQuiz>()
               .Where(uq => uq.CourseId == courseId)
                         .ToListAsync();
        }

        public async Task<IEnumerable<UserQuiz>> GetByUserAndCourseAsync(Guid userId, Guid courseId)
        {
            return await _context.Set<UserQuiz>()
          .Where(uq => uq.UserId == userId && uq.CourseId == courseId)
       .ToListAsync();
        }

        public async Task<UserQuiz?> GetByUserAndQuizAsync(Guid userId, Guid quizId)
        {
            return await _context.Set<UserQuiz>()
              .FirstOrDefaultAsync(uq => uq.UserId == userId && uq.QuizId == quizId);
        }

        
        


      
        public async Task<bool> ExistsAsync(Guid userId, Guid quizId)
        {
            return await _context.Set<UserQuiz>()
                  .AnyAsync(uq => uq.UserId == userId && uq.QuizId == quizId);
        }
    }
}
