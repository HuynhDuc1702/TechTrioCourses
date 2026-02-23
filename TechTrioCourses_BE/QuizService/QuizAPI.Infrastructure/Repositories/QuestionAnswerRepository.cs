using Microsoft.EntityFrameworkCore;
using QuizAPI.Infrastructure.Data;
using QuizAPI.Domain.Entities;
using QuizAPI.Application.Interfaces.IRepositories;
using TechTrioCourses.Shared.Repositories;

namespace QuizAPI.Infrastructure.Repositories
{
    public class QuestionAnswerRepository : GenericRepository<QuestionAnswer, QuizDbContext>,IQuestionAnswerRepository
    {
      
        public QuestionAnswerRepository(QuizDbContext context) : base(context) 
        {
           
        }
        protected override DbSet<QuestionAnswer> DbSet => _context.QuestionAnswers;

        
        public async Task<IEnumerable<QuestionAnswer>> GetByQuestionIdAsync(Guid questionId)
        {
            return await _context.QuestionAnswers
            .Where(qa => qa.QuestionId == questionId)
            .ToListAsync();
        }

      

       

     

      
    }
}
