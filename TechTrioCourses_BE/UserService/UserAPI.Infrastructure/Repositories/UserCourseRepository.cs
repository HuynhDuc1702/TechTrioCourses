using Microsoft.EntityFrameworkCore;
using UserAPI.Infrastructure.Data;
using UserAPI.Domain.Entities;
using UserAPI.Application.Interfaces.IRepositories;
using TechTrioCourses.Shared.Repositories;

namespace UserAPI.Infrastructure.Repositories
{
    public class UserCourseRepository
        : GenericRepository<UserCourse, UserDbContext>,
          IUserCourseRepository
    {
        public UserCourseRepository(UserDbContext context)
            : base(context)
        {
        }

        protected override DbSet<UserCourse> DbSet => _context.UserCourses;

        public async Task<IEnumerable<UserCourse>> GetByUserIdAsync(Guid userId)
        {
            return await DbSet
                .Where(uc => uc.UserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<UserCourse>> GetByCourseIdAsync(Guid courseId)
        {
            return await DbSet
                .Where(uc => uc.CourseId == courseId)
                .ToListAsync();
        }

        public async Task<UserCourse?> GetByUserAndCourseAsync(Guid userId, Guid courseId)
        {
            return await DbSet
                .FirstOrDefaultAsync(uc =>
                    uc.UserId == userId &&
                    uc.CourseId == courseId);
        }

        public async Task<bool> ExistsAsync(Guid userId, Guid courseId)
        {
            return await DbSet
                .AnyAsync(uc =>
                    uc.UserId == userId &&
                    uc.CourseId == courseId);
        }
    }
}