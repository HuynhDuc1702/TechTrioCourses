using TechTrioCourses.Shared.Repositories.Interfaces;
using UserAPI.Domain.Entities;


namespace UserAPI.Application.Interfaces.IRepositories
{
    public interface IUserQuizRepository :IGenericRepository<UserQuiz>
    {
       
        Task<IEnumerable<UserQuiz>> GetByUserIdAsync(Guid userId);
        Task<IEnumerable<UserQuiz>> GetByQuizIdAsync(Guid quizId);
        Task<IEnumerable<UserQuiz>> GetByCourseIdAsync(Guid courseId);
        Task<IEnumerable<UserQuiz>> GetByUserAndCourseAsync(Guid userId, Guid courseId);
        Task<UserQuiz?> GetByUserAndQuizAsync(Guid userId, Guid quizId);
        
        Task<bool> ExistsAsync(Guid userId, Guid quizId);
    }
}
