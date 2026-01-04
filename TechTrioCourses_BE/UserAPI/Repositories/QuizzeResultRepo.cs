using Microsoft.EntityFrameworkCore;
using UserAPI.Datas;
using UserAPI.Models;
using UserAPI.Repositories.Interfaces;

namespace UserAPI.Repositories
{
    public class QuizzeResultRepo : IQuizzeResultRepo
    {
 private readonly TechTrioUsersContext _context;

        public QuizzeResultRepo(TechTrioUsersContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<QuizzeResult>> GetAllAsync()
        {
            return await _context.QuizzeResults.ToListAsync();
    }

        public async Task<QuizzeResult?> GetByIdAsync(Guid id)
        {
      return await _context.QuizzeResults.FirstOrDefaultAsync(qr => qr.Id == id);
        }

        public async Task<IEnumerable<QuizzeResult>> GetByUserIdAsync(Guid userId)
    {
            return await _context.QuizzeResults
       .Where(qr => qr.UserId == userId)
   .OrderByDescending(qr => qr.StartedAt)
                .ToListAsync();
        }

      public async Task<IEnumerable<QuizzeResult>> GetByQuizIdAsync(Guid quizId)
        {
     return await _context.QuizzeResults
                .Where(qr => qr.QuizId == quizId)
    .OrderByDescending(qr => qr.StartedAt)
            .ToListAsync();
        }

  public async Task<IEnumerable<QuizzeResult>> GetByUserAndQuizIdAsync(Guid userId, Guid quizId)
        {
            return await _context.QuizzeResults
   .Where(qr => qr.UserId == userId && qr.QuizId == quizId)
         .OrderByDescending(qr => qr.AttemptNumber)
          .ToListAsync();
        }

        public async Task<QuizzeResult> CreateAsync(QuizzeResult quizzeResult)
 {
         quizzeResult.Id = Guid.NewGuid();
  quizzeResult.StartedAt = DateTime.UtcNow;
            quizzeResult.UpdatedAt = DateTime.UtcNow;

     _context.QuizzeResults.Add(quizzeResult);
            await _context.SaveChangesAsync();

  return quizzeResult;
    }

     public async Task<QuizzeResult?> UpdateAsync(QuizzeResult quizzeResult)
    {
     var existingResult = await _context.QuizzeResults.FindAsync(quizzeResult.Id);
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
          var quizzeResult = await _context.QuizzeResults.FindAsync(id);
            if (quizzeResult == null)
     {
         return false;
            }

          _context.QuizzeResults.Remove(quizzeResult);
            await _context.SaveChangesAsync();

        return true;
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.QuizzeResults.AnyAsync(qr => qr.Id == id);
 }
    }
}
