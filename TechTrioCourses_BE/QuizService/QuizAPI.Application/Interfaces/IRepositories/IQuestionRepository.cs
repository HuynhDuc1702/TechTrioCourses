using QuizAPI.Domain.Entities;
using TechTrioCourses.Shared.Repositories.Interfaces;

namespace QuizAPI.Application.Interfaces.IRepositories
{
    public interface IQuestionRepository : IGenericRepository<Question>
    {
        Task<IEnumerable<Question>> GetQuestionsByCourseId(Guid courseId);
      
    }
}
