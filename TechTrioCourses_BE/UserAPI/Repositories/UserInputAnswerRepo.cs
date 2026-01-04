using Microsoft.EntityFrameworkCore;
using UserAPI.Datas;
using UserAPI.Models;
using UserAPI.Repositories.Interfaces;

namespace UserAPI.Repositories
{
    public class UserInputAnswerRepo : IUserInputAnswerRepo
    {
        private readonly TechTrioUsersContext _context;

        public UserInputAnswerRepo(TechTrioUsersContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserInputAnswer>> GetAllAsync()
     {
            return await _context.UserInputAnswers.ToListAsync();
        }

        public async Task<UserInputAnswer?> GetByIdAsync(Guid id)
   {
            return await _context.UserInputAnswers.FirstOrDefaultAsync(uia => uia.Id == id);
 }

        public async Task<IEnumerable<UserInputAnswer>> GetByResultIdAsync(Guid resultId)
   {
  return await _context.UserInputAnswers
   .Where(uia => uia.ResultId == resultId)
    .ToListAsync();
        }

  public async Task<UserInputAnswer?> GetByResultAndQuestionIdAsync(Guid resultId, Guid questionId)
        {
            return await _context.UserInputAnswers
          .FirstOrDefaultAsync(uia => uia.ResultId == resultId && uia.QuestionId == questionId);
      }

        public async Task<UserInputAnswer> CreateAsync(UserInputAnswer userInputAnswer)
  {
            userInputAnswer.Id = Guid.NewGuid();
   userInputAnswer.CreatedAt = DateTime.UtcNow;
     userInputAnswer.UpdatedAt = DateTime.UtcNow;

            _context.UserInputAnswers.Add(userInputAnswer);
      await _context.SaveChangesAsync();

    return userInputAnswer;
 }

        public async Task<UserInputAnswer?> UpdateAsync(UserInputAnswer userInputAnswer)
        {
            var existingAnswer = await _context.UserInputAnswers.FindAsync(userInputAnswer.Id);
         if (existingAnswer == null)
            {
           return null;
            }

          userInputAnswer.UpdatedAt = DateTime.UtcNow;
   _context.Entry(existingAnswer).CurrentValues.SetValues(userInputAnswer);

            try
        {
      await _context.SaveChangesAsync();
    return existingAnswer;
   }
        catch (DbUpdateConcurrencyException)
     {
       if (!await ExistsAsync(userInputAnswer.Id))
          {
    return null;
        }
         throw;
   }
  }

        public async Task<bool> DeleteAsync(Guid id)
        {
  var userInputAnswer = await _context.UserInputAnswers.FindAsync(id);
  if (userInputAnswer == null)
   {
         return false;
      }

            _context.UserInputAnswers.Remove(userInputAnswer);
      await _context.SaveChangesAsync();

            return true;
        }

  public async Task<bool> ExistsAsync(Guid id)
        {
     return await _context.UserInputAnswers.AnyAsync(uia => uia.Id == id);
        }
    }
}
