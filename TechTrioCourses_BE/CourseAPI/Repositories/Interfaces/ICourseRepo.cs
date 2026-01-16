using CourseAPI.Models;

namespace CourseAPI.Repositories.Interfaces
{
    public interface ICourseRepo
    {
        Task<IEnumerable<Course>> GetAllAsync();
        Task<Course?> GetByIdAsync(Guid id);
        Task<Course> CreateAsync(Course course);
        Task<Course?> UpdateAsync(Course course);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
    }
}
