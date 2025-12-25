using UserAPI.DTOs.Request;
using UserAPI.DTOs.Response;

namespace UserAPI.Services.Interfaces
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
