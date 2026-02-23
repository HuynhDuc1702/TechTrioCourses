using Microsoft.EntityFrameworkCore;
using QuizAPI.Infrastructure.Data;
using QuizAPI.Domain.Entities;
using QuizAPI.Application.Interfaces.IRepositories;
using TechTrioCourses.Shared.Repositories;

namespace QuizAPI.Infrastructure.Repositories
{
    public class QuizRepository : GenericRepository<Quiz,QuizDbContext>,IQuizRepository
    {
     

        public QuizRepository(QuizDbContext context) : base(context) 
        {
        
        }
        protected override DbSet<Quiz> DbSet => _context.Quizzes;
       

        public async Task<IEnumerable<Quiz>> GetByCourseIdAsync(Guid courseId)
        {
            return await _context.Quizzes
                .Include(q => q.QuizQuestions)
                .Where(q => q.CourseId == courseId)
                .ToListAsync();
        }

      

      
    }
}
