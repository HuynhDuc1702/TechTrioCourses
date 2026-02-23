using Microsoft.EntityFrameworkCore;
using UserAPI.Infrastructure.Data;
using UserAPI.Domain.Entities;
using UserAPI.Application.Interfaces.IRepositories;
using TechTrioCourses.Shared.Repositories;

namespace UserAPI.Infrastructure.Repositories
{
    public class UserSelectedChoiceRepository :GenericRepository<UserSelectedChoice, UserDbContext>, IUserSelectedChoiceRepository
    {
     

        public UserSelectedChoiceRepository(UserDbContext context): base(context) 
        {
       
        }
        protected override DbSet<UserSelectedChoice> DbSet => _context.UserSelectedChoices;

      
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

      
      
      
    }
}
