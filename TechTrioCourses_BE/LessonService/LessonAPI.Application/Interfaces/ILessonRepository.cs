using LessonAPI.Domain.Entities;
using TechTrioCourses.Shared.Repositories.Interfaces;

namespace LessonAPI.Application.Interfaces
{
    public interface ILessonRepository : IGenericRepository<Lesson>
    {
        
        Task<IEnumerable<Lesson>> GetAllLessonByCourseAsync(Guid courseId);
       
    }
}
