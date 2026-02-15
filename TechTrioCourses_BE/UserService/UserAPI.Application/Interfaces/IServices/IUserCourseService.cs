using System.Linq.Expressions;
using UserAPI.Application.DTOs.Response.UserCourse;
using UserAPI.Application.DTOs.Request.UserCourse;

namespace UserAPI.Application.Interfaces.IServices
{
    public interface IUserCourseService
    {
        Task<UserCourseResponse?> GetUserCourseByIdAsync(Guid id);
        Task<IEnumerable<UserCourseResponse>> GetAllUserCoursesAsync();
        Task<IEnumerable<UserCourseResponse>> GetUserCoursesByUserIdAsync(Guid userId);
        Task<IEnumerable<UserCourseResponse>> GetUserCoursesByCourseIdAsync(Guid courseId);
        Task<UserCourseResponse?> GetUserCourseByUserAndCourseAsync(Guid userId, Guid courseId);
        Task<UserCourseResponse?> CreateUserCourseAsync(CreateUserCourseRequest request);
        Task<UserCourseResponse?> UpdateUserCourseAsync(Guid id);

        Task<bool> DeleteUserCourseAsync(Guid id);
    }
}
