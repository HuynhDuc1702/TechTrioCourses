using QuizAPI.Domain.Entities;
using TechTrioCourses.Shared.Repositories.Interfaces;

namespace QuizAPI.Application.Interfaces.IRepositories
{
    public interface IQuizRepository : IGenericRepository <Quiz>
    {
      
        Task<IEnumerable<Quiz>> GetByCourseIdAsync(Guid courseId);
       
    }
}
