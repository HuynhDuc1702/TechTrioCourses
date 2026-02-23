using LessonAPI.Infrastructure.Data;
using LessonAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using LessonAPI.Application.Interfaces;
using TechTrioCourses.Shared.Repositories;

namespace LessonAPI.Infrastructure.Repositories
{
    public class LessonRepository : GenericRepository<Lesson,LessonDbContext>,ILessonRepository
    {
       

        public LessonRepository(LessonDbContext context) :base(context) {
        
           
        }
        protected override DbSet<Lesson> DbSet => _context.Lessons;
        
        public async Task<IEnumerable<Lesson>> GetAllLessonByCourseAsync(Guid courseId)
        {
            return await _context.Lessons
                .Where(l=> l.CourseId == courseId)
                .OrderBy(l=>l.CourseId)
                .ToListAsync();
        }

       
    }
}
