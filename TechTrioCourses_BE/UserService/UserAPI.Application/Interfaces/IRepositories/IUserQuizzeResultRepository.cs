using UserAPI.Domain.Entities;

namespace UserAPI.Application.Interfaces.IRepositories
{
    public interface IUserQuizzeResultRepository
    {
        Task<IEnumerable<UserQuizzeResult>> GetAllAsync();
        Task<UserQuizzeResult?> GetByIdAsync(Guid id);
        Task<IEnumerable<UserQuizzeResult>> GetByUserIdAsync(Guid userId);
        Task<IEnumerable<UserQuizzeResult>> GetByQuizIdAsync(Guid quizId);
        Task<IEnumerable<UserQuizzeResult>> GetByUserAndQuizIdAsync(Guid userId, Guid quizId);
        Task<IEnumerable<UserQuizzeResult>> GetByUserQuizIdAsync(Guid userQuizId);
        Task<UserQuizzeResult?> GetLatestByUserQuizIdAsync(Guid userQuizId);
        Task<UserQuizzeResult> CreateAsync(UserQuizzeResult quizzeResult);
        Task<UserQuizzeResult?> UpdateAsync(UserQuizzeResult quizzeResult);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
    }
}
