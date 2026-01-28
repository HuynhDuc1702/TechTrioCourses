using System.Linq.Expressions;
using UserAPI.Models;

namespace UserAPI.Repositories.Interfaces
{
    public interface IUserCourseRepo
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
