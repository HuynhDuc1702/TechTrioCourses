using System.Linq.Expressions;
using UserAPI.Domain.Entities;

namespace UserAPI.Application.Interfaces.IRepositories
{
    public interface IUserCourseRepository
    {
        Task<UserCourse?> GetByIdAsync(Guid id);
        Task<IEnumerable<UserCourse>> GetAllAsync();
        Task<IEnumerable<UserCourse>> GetByUserIdAsync(Guid userId);
        Task<IEnumerable<UserCourse>> GetByCourseIdAsync(Guid courseId);
        Task<UserCourse?> GetByUserAndCourseAsync(Guid userId, Guid courseId);
        Task<UserCourse> CreateUserCourseAsync(UserCourse userCourse);
        Task<bool> UpdateUserCourseAsync(UserCourse userCourse);
        Task<bool> DeleteUserCourseAsync(Guid id);
        Task<bool> UserCourseExistsAsync(Guid userId, Guid courseId);
    }
}
