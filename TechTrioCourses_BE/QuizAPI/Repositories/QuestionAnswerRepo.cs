using Microsoft.EntityFrameworkCore;
using QuizAPI.Datas;
using QuizAPI.Models;
using QuizAPI.Repositories.Interfaces;

namespace QuizAPI.Repositories
{
    public class QuestionAnswerRepo : IQuestionAnswerRepo
    {
        private readonly QuizzesContext _context;

   public QuestionAnswerRepo(QuizzesContext context)
      {
      _context = context;
 }

  public async Task<IEnumerable<QuestionAnswer>> GetAllAsync()
        {
     return await _context.QuestionAnswers.ToListAsync();
     }

        public async Task<QuestionAnswer?> GetByIdAsync(Guid id)
 {
   return await _context.QuestionAnswers.FirstOrDefaultAsync(qa => qa.Id == id);
     }

  public async Task<IEnumerable<QuestionAnswer>> GetByQuestionIdAsync(Guid questionId)
        {
       return await _context.QuestionAnswers
     .Where(qa => qa.QuestionId == questionId)
  .ToListAsync();
}

     public async Task<QuestionAnswer> CreateAsync(QuestionAnswer questionAnswer)
   {
   questionAnswer.Id = Guid.NewGuid();
            questionAnswer.CreatedAt = DateTime.UtcNow;
      questionAnswer.UpdatedAt = DateTime.UtcNow;

            _context.QuestionAnswers.Add(questionAnswer);
       await _context.SaveChangesAsync();

  return questionAnswer;
 }

 public async Task<QuestionAnswer?> UpdateAsync(QuestionAnswer questionAnswer)
        {
var existingAnswer = await _context.QuestionAnswers.FindAsync(questionAnswer.Id);
if (existingAnswer == null)
            {
   return null;
            }

 questionAnswer.UpdatedAt = DateTime.UtcNow;
         _context.Entry(existingAnswer).CurrentValues.SetValues(questionAnswer);

     try
            {
      await _context.SaveChangesAsync();
      return existingAnswer;
 }
catch (DbUpdateConcurrencyException)
        {
       if (!await ExistsAsync(questionAnswer.Id))
    {
    return null;
     }
 throw;
       }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var questionAnswer = await _context.QuestionAnswers.FindAsync(id);
   if (questionAnswer == null)
 {
          return false;
       }

   _context.QuestionAnswers.Remove(questionAnswer);
     await _context.SaveChangesAsync();

     return true;
        }

    public async Task<bool> ExistsAsync(Guid id)
 {
       return await _context.QuestionAnswers.AnyAsync(qa => qa.Id == id);
  }
    }
}
