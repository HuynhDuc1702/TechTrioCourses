using Microsoft.EntityFrameworkCore;
using QuizAPI.Infrastructure.Data;
using QuizAPI.Domain.Entities;
using QuizAPI.Application.Interfaces.IRepositories;
using TechTrioCourses.Shared.Repositories;

namespace QuizAPI.Infrastructure.Repositories
{
    public class QuestionChoiceRepository : GenericRepository<QuestionChoice,QuizDbContext>,IQuestionChoiceRepository
    {
 

        public QuestionChoiceRepository(QuizDbContext context) : base(context) 
        {
            
        }
        protected override DbSet<QuestionChoice> DbSet => _context.QuestionChoices;

      
        public async Task<IEnumerable<QuestionChoice>> GetByQuestionIdAsync(Guid questionId)
        {
            return await _context.QuestionChoices
        .Where(qc => qc.QuestionId == questionId)
                .ToListAsync();
        }
      
      
    }
}
