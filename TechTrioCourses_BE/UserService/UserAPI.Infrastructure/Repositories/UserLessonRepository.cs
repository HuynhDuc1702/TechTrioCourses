using Microsoft.EntityFrameworkCore;
using UserAPI.Infrastructure.Data;
using UserAPI.Domain.Entities;
using UserAPI.Application.Interfaces.IRepositories;
using TechTrioCourses.Shared.Repositories;

namespace UserAPI.Infrastructure.Repositories
{
    public class UserLessonRepository : GenericRepository<UserLesson, UserDbContext>, IUserLessonRepository
    {


        public UserLessonRepository(UserDbContext context) : base(context)
        {

        }
        protected override DbSet<UserLesson> DbSet => _context.UserLessons;

        public async Task<IEnumerable<UserLesson>> GetByUserIdAsync(Guid userId)
        {
            return await _context.Set<UserLesson>()
.Where(ul => ul.UserId == userId)
      .ToListAsync();
        }

        public async Task<IEnumerable<UserLesson>> GetByLessonIdAsync(Guid lessonId)
        {
            return await _context.Set<UserLesson>()
               .Where(ul => ul.LessonId == lessonId)
                          .ToListAsync();
        }

        public async Task<UserLesson?> GetByUserAndLessonAsync(Guid userId, Guid lessonId)
        {
            return await _context.Set<UserLesson>()
             .FirstOrDefaultAsync(ul => ul.UserId == userId && ul.LessonId == lessonId);
        }

      
      
      
        public async Task<bool> ExistsAsync(Guid userId, Guid lessonId)
        {
            return await _context.Set<UserLesson>()
        .AnyAsync(ul => ul.UserId == userId && ul.LessonId == lessonId);
        }

        public async Task<IEnumerable<UserLesson>> GetByCourseIdAsync(Guid courseId)
        {
            return await _context.Set<UserLesson>()
     .Where(ul => ul.CourseId == courseId)
       .ToListAsync();
        }

        public async Task<IEnumerable<UserLesson>> GetByUserAndCourseAsync(Guid userId, Guid courseId)
        {
            return await _context.Set<UserLesson>()
                .Where(ul => ul.UserId == userId && ul.CourseId == courseId)
                    .ToListAsync();
        }
    }
}
