using Microsoft.EntityFrameworkCore;
using Polly;
using TechTrioCourses.Shared.Repositories;
using UserAPI.Application.Interfaces.IRepositories;
using UserAPI.Domain.Entities;
using UserAPI.Infrastructure.Data;

namespace UserAPI.Infrastructure.Repositories
{
    public class UserInputAnswerRepository : GenericRepository<UserInputAnswer, UserDbContext>,IUserInputAnswerRepository
    {


        public UserInputAnswerRepository(UserDbContext context) : base(context)
        {
        
         
        }
        protected override DbSet<UserInputAnswer> DbSet => _context.UserInputAnswers;
       
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

      
    }
}
