using System.Linq.Expressions;
using TechTrioCourses.Shared.Repositories.Interfaces;
using UserAPI.Domain.Entities;

namespace UserAPI.Application.Interfaces.IRepositories
{
    public interface IUserCourseRepository : IGenericRepository<UserCourse>
    {
  
        Task<IEnumerable<UserCourse>> GetByUserIdAsync(Guid userId);
        Task<IEnumerable<UserCourse>> GetByCourseIdAsync(Guid courseId);
        Task<UserCourse?> GetByUserAndCourseAsync(Guid userId, Guid courseId);
      
        Task<bool> ExistsAsync(Guid userId, Guid courseId);
    }
}
