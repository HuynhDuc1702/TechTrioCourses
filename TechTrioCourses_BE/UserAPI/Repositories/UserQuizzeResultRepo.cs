using Microsoft.EntityFrameworkCore;
using TechTrioCourses.Shared.Enums;
using UserAPI.Datas;
using UserAPI.Models;
using UserAPI.Repositories.Interfaces;

namespace UserAPI.Repositories
{
    public class UserQuizzeResultRepo : IUserQuizzeResultRepo
    {
        private readonly TechTrioUsersContext _context;

        public UserQuizzeResultRepo(TechTrioUsersContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserQuizzeResult>> GetAllAsync()
        {
            return await _context.UserQuizzeResults.ToListAsync();
        }

        public async Task<UserQuizzeResult?> GetByIdAsync(Guid id)
        {
            return await _context.UserQuizzeResults.FirstOrDefaultAsync(qr => qr.Id == id);
        }

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


        public async Task<UserQuizzeResult> CreateAsync(UserQuizzeResult quizzeResult)
        {
            quizzeResult.Id = Guid.NewGuid();
            quizzeResult.StartedAt = DateTime.UtcNow;
            quizzeResult.UpdatedAt = DateTime.UtcNow;
            quizzeResult.Status = UserQuizResultStatusEnum.In_progress;

            _context.UserQuizzeResults.Add(quizzeResult);
            await _context.SaveChangesAsync();

            return quizzeResult;
        }

        public async Task<UserQuizzeResult?> UpdateAsync(UserQuizzeResult quizzeResult)
        {
            var existingResult = await _context.UserQuizzeResults.FindAsync(quizzeResult.Id);
            if (existingResult == null)
            {
                return null;
            }

            quizzeResult.UpdatedAt = DateTime.UtcNow;
            _context.Entry(existingResult).CurrentValues.SetValues(quizzeResult);

            try
            {
                await _context.SaveChangesAsync();
                return existingResult;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ExistsAsync(quizzeResult.Id))
                {
                    return null;
                }
                throw;
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var quizzeResult = await _context.UserQuizzeResults.FindAsync(id);
            if (quizzeResult == null)
            {
                return false;
            }

            _context.UserQuizzeResults.Remove(quizzeResult);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.UserQuizzeResults.AnyAsync(qr => qr.Id == id);
        }
    }
}
