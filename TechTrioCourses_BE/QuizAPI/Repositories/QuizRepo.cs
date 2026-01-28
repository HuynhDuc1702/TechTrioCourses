using Microsoft.EntityFrameworkCore;
using QuizAPI.Datas;
using QuizAPI.Models;
using QuizAPI.Repositories.Interfaces;

namespace QuizAPI.Repositories
{
    public class QuizRepo : IQuizRepo
    {
        private readonly QuizzesContext _context;

        public QuizRepo(QuizzesContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Quiz>> GetAllAsync()
        {
            return await _context.Quizzes.Include(q => q.QuizQuestions).ToListAsync();
        }

        public async Task<Quiz?> GetByIdAsync(Guid id)
        {
            return await _context.Quizzes.Include(q => q.QuizQuestions).FirstOrDefaultAsync(q => q.Id == id);
        }

        public async Task<IEnumerable<Quiz>> GetByCourseIdAsync(Guid courseId)
        {
            return await _context.Quizzes
                .Include(q => q.QuizQuestions)
                .Where(q => q.CourseId == courseId)
                .ToListAsync();
        }

        public async Task<Quiz> CreateAsync(Quiz quiz)
        {
            quiz.Id = Guid.NewGuid();
            quiz.CreatedAt = DateTime.UtcNow;
            quiz.UpdatedAt = DateTime.UtcNow;

            _context.Quizzes.Add(quiz);
            await _context.SaveChangesAsync();

            return quiz;
        }

        public async Task<Quiz?> UpdateAsync(Quiz quiz)
        {
            var existingQuiz = await _context.Quizzes.FindAsync(quiz.Id);
            if (existingQuiz == null)
            {
                return null;
            }

            quiz.UpdatedAt = DateTime.UtcNow;
            _context.Entry(existingQuiz).CurrentValues.SetValues(quiz);

            try
            {
                await _context.SaveChangesAsync();
                return existingQuiz;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ExistsAsync(quiz.Id))
                {
                    return null;
                }
                throw;
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var quiz = await _context.Quizzes.FindAsync(id);
            if (quiz == null)
            {
                return false;
            }

            _context.Quizzes.Remove(quiz);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Quizzes.AnyAsync(q => q.Id == id);
        }
    }
}
