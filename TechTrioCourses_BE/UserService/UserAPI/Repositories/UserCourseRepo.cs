using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TechTrioCourses.Shared.Enums;
using UserAPI.Datas;
using UserAPI.Models;
using UserAPI.Repositories.Interfaces;

namespace UserAPI.Repositories
{
    public class UserCourseRepo : IUserCourseRepo
    {
        private readonly UserDbContext _context;

        public UserCourseRepo(UserDbContext context)
        {
            _context = context;
        }

        public async Task<UserCourse?> GetByIdAsync(Guid id)
        {
            return await _context.Set<UserCourse>().FirstOrDefaultAsync(uc => uc.Id == id);
        }

        public async Task<IEnumerable<UserCourse>> GetAllAsync()
        {
            return await _context.Set<UserCourse>().ToListAsync();
        }

        public async Task<IEnumerable<UserCourse>> GetByUserIdAsync(Guid userId)
        {
            return await _context.Set<UserCourse>()
                .Where(uc => uc.UserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<UserCourse>> GetByCourseIdAsync(Guid courseId)
        {
            return await _context.Set<UserCourse>()
                .Where(uc => uc.CourseId == courseId)
                .ToListAsync();
        }

        public async Task<UserCourse?> GetByUserAndCourseAsync(Guid userId, Guid courseId)
        {
            return await _context.Set<UserCourse>().FirstOrDefaultAsync(uc => uc.CourseId == courseId && uc.UserId == userId);
        }

        public async Task<UserCourse> CreateUserCourseAsync(UserCourse userCourse)
        {
            userCourse.Id = Guid.NewGuid();
            userCourse.EnrolledAt = DateTime.UtcNow;
            userCourse.UpdatedAt = DateTime.UtcNow;
            userCourse.Progress = 0;
            userCourse.Status = UserCourseStatusEnum.In_progress;

            _context.Set<UserCourse>().Add(userCourse);
            await _context.SaveChangesAsync();

            return userCourse;
        }

        public async Task<bool> UpdateUserCourseAsync(UserCourse userCourse)
        {
            userCourse.UpdatedAt = DateTime.UtcNow;

            _context.Set<UserCourse>().Update(userCourse);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteUserCourseAsync(Guid id)
        {
            var userCourse = await GetByIdAsync(id);
            if (userCourse == null)
            {
                return false;
            }

            _context.Set<UserCourse>().Remove(userCourse);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UserCourseExistsAsync(Guid userId, Guid courseId)
        {
            return await _context.Set<UserCourse>()
                .AnyAsync(uc => uc.UserId == userId && uc.CourseId == courseId);
        }
    }
}
