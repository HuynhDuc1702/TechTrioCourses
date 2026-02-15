using LessonAPI.Domain.Entities;

namespace LessonAPI.Application.Interfaces
{
    public interface ILessonRepository
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
