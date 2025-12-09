using Microsoft.EntityFrameworkCore;
using QuizAPI.Datas;
using QuizAPI.Models;
using QuizAPI.Repositories.Interfaces;

namespace QuizAPI.Repositories
{
    public class QuestionRepo : IQuestionRepo
    {
        private readonly QuizzesContext _context;

        public QuestionRepo(QuizzesContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Question>> GetAllAsync()
        {
            return await _context.Questions.ToListAsync();
        }

        public async Task<Question?> GetByIdAsync(Guid id)
        {
            return await _context.Questions.FirstOrDefaultAsync(q => q.Id == id);
        }

        public async Task<Question> CreateAsync(Question question)
        {
            question.Id = Guid.NewGuid();
            question.CreatedAt = DateTime.UtcNow;
            question.UpdatedAt = DateTime.UtcNow;

            _context.Questions.Add(question);
            await _context.SaveChangesAsync();

            return question;
        }

        public async Task<Question?> UpdateAsync(Question question)
        {
            var existingQuestion = await _context.Questions.FindAsync(question.Id);
            if (existingQuestion == null)
            {
                return null;
            }

            question.UpdatedAt = DateTime.UtcNow;
            _context.Entry(existingQuestion).CurrentValues.SetValues(question);

            try
            {
                await _context.SaveChangesAsync();
                return existingQuestion;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ExistsAsync(question.Id))
                {
                    return null;
                }
                throw;
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var question = await _context.Questions.FindAsync(id);
            if (question == null)
            {
                return false;
            }

            _context.Questions.Remove(question);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Questions.AnyAsync(q => q.Id == id);
        }
    }
}
