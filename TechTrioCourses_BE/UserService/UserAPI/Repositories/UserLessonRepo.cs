using Microsoft.EntityFrameworkCore;
using TechTrioCourses.Shared.Enums;
using UserAPI.Datas;
using UserAPI.Models;
using UserAPI.Repositories.Interfaces;

namespace UserAPI.Repositories
{
    public class UserLessonRepo : IUserLessonRepo
    {
        private readonly UserDbContext _context;

        public UserLessonRepo(UserDbContext context)
        {
            _context = context;
        }

        public async Task<UserLesson?> GetByIdAsync(Guid id)
        {
            return await _context.Set<UserLesson>().FirstOrDefaultAsync(ul => ul.Id == id);
        }

        public async Task<IEnumerable<UserLesson>> GetAllAsync()
        {
            return await _context.Set<UserLesson>().ToListAsync();
        }

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

        public async Task<UserLesson> CreateUserLessonAsync(UserLesson userLesson)
        {
            userLesson.Id = Guid.NewGuid();
            userLesson.UpdatedAt = DateTime.UtcNow;

            _context.Set<UserLesson>().Add(userLesson);
            await _context.SaveChangesAsync();

            return userLesson;
        }

        public async Task<bool> UpdateUserLessonAsync(UserLesson userLesson)
        {
            userLesson.UpdatedAt = DateTime.UtcNow;
            _context.Set<UserLesson>().Update(userLesson);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteUserLessonAsync(Guid id)
        {
            var userLesson = await GetByIdAsync(id);
            if (userLesson == null)
            {
                return false;
            }

            _context.Set<UserLesson>().Remove(userLesson);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UserLessonExistsAsync(Guid userId, Guid lessonId)
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
