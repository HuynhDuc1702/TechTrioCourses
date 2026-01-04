using Microsoft.EntityFrameworkCore;
using UserAPI.Datas;
using UserAPI.Models;
using UserAPI.Repositories.Interfaces;

namespace UserAPI.Repositories
{
    public class UserSelectedChoiceRepo : IUserSelectedChoiceRepo
    {
        private readonly TechTrioUsersContext _context;

        public UserSelectedChoiceRepo(TechTrioUsersContext context)
        {
         _context = context;
        }

        public async Task<IEnumerable<UserSelectedChoice>> GetAllAsync()
      {
    return await _context.UserSelectedChoices.ToListAsync();
        }

        public async Task<UserSelectedChoice?> GetByIdAsync(Guid id)
        {
       return await _context.UserSelectedChoices.FirstOrDefaultAsync(usc => usc.Id == id);
        }

        public async Task<IEnumerable<UserSelectedChoice>> GetByResultIdAsync(Guid resultId)
        {
 return await _context.UserSelectedChoices
           .Where(usc => usc.ResultId == resultId)
          .ToListAsync();
  }

      public async Task<UserSelectedChoice?> GetByResultAndQuestionIdAsync(Guid resultId, Guid questionId)
     {
       return await _context.UserSelectedChoices
   .FirstOrDefaultAsync(usc => usc.ResultId == resultId && usc.QuestionId == questionId);
        }

        public async Task<UserSelectedChoice> CreateAsync(UserSelectedChoice userSelectedChoice)
        {
            userSelectedChoice.Id = Guid.NewGuid();
    userSelectedChoice.CreatedAt = DateTime.UtcNow;
            userSelectedChoice.UpdatedAt = DateTime.UtcNow;

          _context.UserSelectedChoices.Add(userSelectedChoice);
     await _context.SaveChangesAsync();

        return userSelectedChoice;
  }

    public async Task<UserSelectedChoice?> UpdateAsync(UserSelectedChoice userSelectedChoice)
    {
            var existingChoice = await _context.UserSelectedChoices.FindAsync(userSelectedChoice.Id);
          if (existingChoice == null)
            {
         return null;
          }

 userSelectedChoice.UpdatedAt = DateTime.UtcNow;
        _context.Entry(existingChoice).CurrentValues.SetValues(userSelectedChoice);

            try
    {
            await _context.SaveChangesAsync();
   return existingChoice;
            }
 catch (DbUpdateConcurrencyException)
 {
  if (!await ExistsAsync(userSelectedChoice.Id))
         {
 return null;
        }
  throw;
 }
  }

        public async Task<bool> DeleteAsync(Guid id)
{
       var userSelectedChoice = await _context.UserSelectedChoices.FindAsync(id);
      if (userSelectedChoice == null)
          {
        return false;
            }

   _context.UserSelectedChoices.Remove(userSelectedChoice);
 await _context.SaveChangesAsync();

  return true;
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
    return await _context.UserSelectedChoices.AnyAsync(usc => usc.Id == id);
        }
  }
}
