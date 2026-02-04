using LessonAPI.Models;

namespace LessonAPI.Repositories.Interfaces
{
    public interface ILessonRepo
    {
        Task<IEnumerable<Lesson>> GetAllAsync();
        Task<IEnumerable<Lesson>> GetAllLessonByCourseAsync(Guid courseId);
        Task<Lesson?> GetByIdAsync(Guid id);
        Task<Lesson> CreateAsync(Lesson course);
        Task<Lesson?> UpdateAsync(Lesson course);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
    }
}
