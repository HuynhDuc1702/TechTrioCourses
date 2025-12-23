using UserAPI.Models;

namespace UserAPI.Repositories.Interfaces
{
    public interface IUserLessonRepo
    {
        Task<UserLesson?> GetByIdAsync(Guid id);
        Task<IEnumerable<UserLesson>> GetAllAsync();
        Task<IEnumerable<UserLesson>> GetByUserIdAsync(Guid userId);
        Task<IEnumerable<UserLesson>> GetByLessonIdAsync(Guid lessonId);
        Task<IEnumerable<UserLesson>> GetByCourseIdAsync(Guid courseId);
        Task<IEnumerable<UserLesson>> GetByUserAndCourseAsync(Guid userId, Guid courseId);
        Task<UserLesson?> GetByUserAndLessonAsync(Guid userId, Guid lessonId);
        Task<UserLesson> CreateUserLessonAsync(UserLesson userLesson);
        Task<bool> UpdateUserLessonAsync(UserLesson userLesson);
        Task<bool> DeleteUserLessonAsync(Guid id);
        Task<bool> UserLessonExistsAsync(Guid userId, Guid lessonId);
    }
}
