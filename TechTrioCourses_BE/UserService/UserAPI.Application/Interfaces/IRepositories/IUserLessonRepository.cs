using TechTrioCourses.Shared.Repositories.Interfaces;
using UserAPI.Domain.Entities;

namespace UserAPI.Application.Interfaces.IRepositories
{
    public interface IUserLessonRepository :IGenericRepository<UserLesson>
    {
        
        Task<IEnumerable<UserLesson>> GetByUserIdAsync(Guid userId);
        Task<IEnumerable<UserLesson>> GetByLessonIdAsync(Guid lessonId);
        Task<IEnumerable<UserLesson>> GetByCourseIdAsync(Guid courseId);
        Task<IEnumerable<UserLesson>> GetByUserAndCourseAsync(Guid userId, Guid courseId);
        Task<UserLesson?> GetByUserAndLessonAsync(Guid userId, Guid lessonId);
     
        Task<bool> ExistsAsync(Guid userId, Guid lessonId);
    }
}
