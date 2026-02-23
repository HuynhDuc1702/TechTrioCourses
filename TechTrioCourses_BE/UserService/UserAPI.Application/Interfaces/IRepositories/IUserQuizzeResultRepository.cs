using TechTrioCourses.Shared.Repositories.Interfaces;
using UserAPI.Domain.Entities;

namespace UserAPI.Application.Interfaces.IRepositories
{
    public interface IUserQuizzeResultRepository :IGenericRepository<UserQuizzeResult>
    {
      
        Task<IEnumerable<UserQuizzeResult>> GetByUserIdAsync(Guid userId);
        Task<IEnumerable<UserQuizzeResult>> GetByQuizIdAsync(Guid quizId);
        Task<IEnumerable<UserQuizzeResult>> GetByUserAndQuizIdAsync(Guid userId, Guid quizId);
        Task<IEnumerable<UserQuizzeResult>> GetByUserQuizIdAsync(Guid userQuizId);
        Task<UserQuizzeResult?> GetLatestByUserQuizIdAsync(Guid userQuizId);
       
    }
}
