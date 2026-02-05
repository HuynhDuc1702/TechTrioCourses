using CourseAPI.Domain.Entities;

namespace CourseAPI.Application.Interfaces
{
    public interface ICourseRepository
    {
        Task<IEnumerable<Course>> GetAllAsync();
        Task<Course?> GetByIdAsync(Guid id);
        Task<Course> CreateAsync(Course course);
        Task<Course?> UpdateAsync(Course course);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
    }
}
