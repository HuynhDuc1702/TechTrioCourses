using UserAPI.Application.DTOs.Request.UserLesson;
using UserAPI.Application.DTOs.Response.UserLesson;

namespace UserAPI.Application.Interfaces.IServices
{
    public interface IUserLessonService
    {
        Task<UserLessonResponse?> GetUserLessonByIdAsync(Guid id);
        Task<IEnumerable<UserLessonResponse>> GetAllUserLessonsAsync();
        Task<IEnumerable<UserLessonResponse>> GetUserLessonsByUserIdAsync(Guid userId);
        Task<IEnumerable<UserLessonResponse>> GetUserLessonsByLessonIdAsync(Guid lessonId);
        Task<IEnumerable<UserLessonResponse>> GetUserLessonsByCourseIdAsync(Guid courseId);
        Task<IEnumerable<UserLessonResponse>> GetUserLessonsByUserAndCourseAsync(Guid userId, Guid courseId);
        Task<UserLessonResponse?> GetUserLessonByUserAndLessonAsync(Guid userId, Guid lessonId);
        Task<UserLessonResponse?> CreateUserLessonAsync(CreateUserLessonRequest request);
        Task<bool> DeleteUserLessonAsync(Guid id);
    }
}
