using UserAPI.DTOs.Request.UserCourse;
using UserAPI.DTOs.Response.UserCourse;
using System.Linq.Expressions;

namespace UserAPI.Services.Interfaces
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
