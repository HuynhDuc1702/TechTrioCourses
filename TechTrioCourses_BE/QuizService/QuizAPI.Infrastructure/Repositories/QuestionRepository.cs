using Microsoft.EntityFrameworkCore;
using QuizAPI.Infrastructure.Data;
using QuizAPI.Domain.Entities;
using QuizAPI.Application.Interfaces.IRepositories;
using TechTrioCourses.Shared.Repositories;

namespace QuizAPI.Infrastructure.Repositories
{
    public class QuestionRepository : GenericRepository<Question,QuizDbContext>,IQuestionRepository
    {
     

        public QuestionRepository(QuizDbContext context) : base (context) 
        {
            
        }
        protected override DbSet<Question> DbSet => _context.Questions;
       
        public async Task<IEnumerable<Question>> GetQuestionsByCourseId( Guid courseId)
        {
            return await _context.Questions
                .Where(q=> q.CourseId==courseId)
                .ToListAsync();
        }

      
       
    }
}
