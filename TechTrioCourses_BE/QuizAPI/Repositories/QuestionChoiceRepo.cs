using Microsoft.EntityFrameworkCore;
using QuizAPI.Datas;
using QuizAPI.Models;
using QuizAPI.Repositories.Interfaces;

namespace QuizAPI.Repositories
{
    public class QuestionChoiceRepo : IQuestionChoiceRepo
    {
        private readonly QuizzesContext _context;

        public QuestionChoiceRepo(QuizzesContext context)
  {
            _context = context;
     }

     public async Task<IEnumerable<QuestionChoice>> GetAllAsync()
        {
return await _context.QuestionChoices.ToListAsync();
      }

        public async Task<QuestionChoice?> GetByIdAsync(Guid id)
        {
          return await _context.QuestionChoices.FirstOrDefaultAsync(qc => qc.Id == id);
    }

        public async Task<IEnumerable<QuestionChoice>> GetByQuestionIdAsync(Guid questionId)
      {
            return await _context.QuestionChoices
        .Where(qc => qc.QuestionId == questionId)
                .ToListAsync();
    }

        public async Task<QuestionChoice> CreateAsync(QuestionChoice questionChoice)
        {
      questionChoice.Id = Guid.NewGuid();
       questionChoice.CreatedAt = DateTime.UtcNow;
          questionChoice.UpdatedAt = DateTime.UtcNow;

            _context.QuestionChoices.Add(questionChoice);
       await _context.SaveChangesAsync();

          return questionChoice;
        }

        public async Task<QuestionChoice?> UpdateAsync(QuestionChoice questionChoice)
        {
      var existingChoice = await _context.QuestionChoices.FindAsync(questionChoice.Id);
  if (existingChoice == null)
    {
     return null;
            }

            questionChoice.UpdatedAt = DateTime.UtcNow;
         _context.Entry(existingChoice).CurrentValues.SetValues(questionChoice);

            try
            {
         await _context.SaveChangesAsync();
           return existingChoice;
 }
 catch (DbUpdateConcurrencyException)
            {
                if (!await ExistsAsync(questionChoice.Id))
           {
        return null;
   }
    throw;
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
   {
       var questionChoice = await _context.QuestionChoices.FindAsync(id);
         if (questionChoice == null)
            {
                return false;
         }

  _context.QuestionChoices.Remove(questionChoice);
     await _context.SaveChangesAsync();

    return true;
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
      return await _context.QuestionChoices.AnyAsync(qc => qc.Id == id);
        }
    }
}
