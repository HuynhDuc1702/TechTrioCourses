using Microsoft.EntityFrameworkCore;
using QuizAPI.Datas;
using QuizAPI.Models;
using QuizAPI.Repositories.Interfaces;

namespace QuizAPI.Repositories
{
    public class QuizQuestionRepo : IQuizQuestionRepo
    {
        private readonly QuizDbContext _context;

        public QuizQuestionRepo(QuizDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<QuizQuestion>> GetAllAsync()
        {
            return await _context.QuizQuestions.ToListAsync();
        }

        public async Task<QuizQuestion?> GetByIdAsync(Guid quizId, Guid questionId)
        {
            return await _context.QuizQuestions
          .FirstOrDefaultAsync(qq => qq.QuizId == quizId && qq.QuestionId == questionId);
        }

        public async Task<IEnumerable<QuizQuestion>> GetByQuizIdAsync(Guid quizId)
        {
            return await _context.QuizQuestions
              .Where(qq => qq.QuizId == quizId)
         .ToListAsync();
        }

        public async Task<IEnumerable<QuizQuestion>> GetByQuestionIdAsync(Guid questionId)
        {
            return await _context.QuizQuestions
              .Where(qq => qq.QuestionId == questionId)
         .ToListAsync();
        }

        public async Task<QuizQuestion> CreateAsync(QuizQuestion quizQuestion)
        {
            _context.QuizQuestions.Add(quizQuestion);
            await _context.SaveChangesAsync();

            return quizQuestion;
        }

        public async Task<QuizQuestion?> UpdateAsync(QuizQuestion quizQuestion)
        {
            var existingQuizQuestion = await _context.QuizQuestions
              .FindAsync(quizQuestion.QuizId, quizQuestion.QuestionId);

            if (existingQuizQuestion == null)
            {
                return null;
            }

            _context.Entry(existingQuizQuestion).CurrentValues.SetValues(quizQuestion);

            try
            {
                await _context.SaveChangesAsync();
                return existingQuizQuestion;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ExistsAsync(quizQuestion.QuizId, quizQuestion.QuestionId))
                {
                    return null;
                }
                throw;
            }
        }

        public async Task<bool> DeleteAsync(Guid quizId, Guid questionId)
        {
            var quizQuestion = await _context.QuizQuestions
        .FindAsync(quizId, questionId);

            if (quizQuestion == null)
            {
                return false;
            }

            _context.QuizQuestions.Remove(quizQuestion);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ExistsAsync(Guid quizId, Guid questionId)
        {
            return await _context.QuizQuestions
                    .AnyAsync(qq => qq.QuizId == quizId && qq.QuestionId == questionId);
        }
    }
}
