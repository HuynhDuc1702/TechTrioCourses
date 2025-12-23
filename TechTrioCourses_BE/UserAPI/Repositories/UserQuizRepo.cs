using Microsoft.EntityFrameworkCore;
using UserAPI.Datas;
using UserAPI.Enums;
using UserAPI.Models;
using UserAPI.Repositories.Interfaces;

namespace UserAPI.Repositories
{
    public class UserQuizRepo : IUserQuizRepo
    {
        private readonly TechTrioUsersContext _context;

        public UserQuizRepo(TechTrioUsersContext context)
        {
            _context = context;
        }

        public async Task<UserQuiz?> GetByIdAsync(Guid id)
        {
            return await _context.Set<UserQuiz>().FirstOrDefaultAsync(uq => uq.Id == id);
        }

        public async Task<IEnumerable<UserQuiz>> GetAllAsync()
        {
            return await _context.Set<UserQuiz>().ToListAsync();
        }

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

        public async Task<UserQuiz> CreateUserQuizAsync(UserQuiz userQuiz)
        {
            userQuiz.Id = Guid.NewGuid();
            userQuiz.UpdatedAt = DateTime.UtcNow;
            userQuiz.Status = UserQuizzStatus.Not_Started;

            _context.Set<UserQuiz>().Add(userQuiz);
            await _context.SaveChangesAsync();

            return userQuiz;
        }

        public async Task<bool> UpdateUserQuizAsync(Guid userId, Guid quizId, Enums.UserQuizzStatus newStatus, double? score)
        {
            var userQuiz = await _context.Set<UserQuiz>()
                .FirstOrDefaultAsync(x => x.UserId == userId && x.QuizId == quizId);

            if (userQuiz == null)
                return false;

            userQuiz.UpdatedAt = DateTime.UtcNow;
            userQuiz.LastAttemptAt = DateTime.UtcNow;

            // Attempt logic
            userQuiz.AttemptCount += 1;

            if (score.HasValue)
            {
                if (!userQuiz.BestScore.HasValue || score > userQuiz.BestScore)
                {
                    userQuiz.BestScore = score;
                }
            }

            // Status transition
            if (newStatus == Enums.UserQuizzStatus.Passed)
            {
                if (userQuiz.Status != Enums.UserQuizzStatus.Passed)
                {
                    userQuiz.PassedAt = DateTime.UtcNow;
                }
            }
            if (userQuiz.Status != UserQuizzStatus.Passed)
            {
                userQuiz.Status = newStatus;
            }
            ;

            return await _context.SaveChangesAsync() > 0;
        }


        public async Task<bool> DeleteUserQuizAsync(Guid id)
        {
            var userQuiz = await GetByIdAsync(id);
            if (userQuiz == null)
            {
                return false;
            }

            _context.Set<UserQuiz>().Remove(userQuiz);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UserQuizExistsAsync(Guid userId, Guid quizId)
        {
            return await _context.Set<UserQuiz>()
                  .AnyAsync(uq => uq.UserId == userId && uq.QuizId == quizId);
        }
    }
}
