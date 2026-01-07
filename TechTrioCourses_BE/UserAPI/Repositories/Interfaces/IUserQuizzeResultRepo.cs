using UserAPI.Models;

namespace UserAPI.Repositories.Interfaces
{
    public interface IUserQuizzeResultRepo
    {
        Task<IEnumerable<UserQuizzeResult>> GetAllAsync();
        Task<UserQuizzeResult?> GetByIdAsync(Guid id);
  Task<IEnumerable<UserQuizzeResult>> GetByUserIdAsync(Guid userId);
        Task<IEnumerable<UserQuizzeResult>> GetByQuizIdAsync(Guid quizId);
  Task<IEnumerable<UserQuizzeResult>> GetByUserAndQuizIdAsync(Guid userId, Guid quizId);
   Task<UserQuizzeResult> CreateAsync(UserQuizzeResult quizzeResult);
    Task<UserQuizzeResult?> UpdateAsync(UserQuizzeResult quizzeResult);
    Task<bool> DeleteAsync(Guid id);
      Task<bool> ExistsAsync(Guid id);
    }
}
