using UserAPI.Models;

namespace UserAPI.Repositories.Interfaces
{
    public interface IQuizzeResultRepo
    {
        Task<IEnumerable<QuizzeResult>> GetAllAsync();
        Task<QuizzeResult?> GetByIdAsync(Guid id);
  Task<IEnumerable<QuizzeResult>> GetByUserIdAsync(Guid userId);
        Task<IEnumerable<QuizzeResult>> GetByQuizIdAsync(Guid quizId);
  Task<IEnumerable<QuizzeResult>> GetByUserAndQuizIdAsync(Guid userId, Guid quizId);
   Task<QuizzeResult> CreateAsync(QuizzeResult quizzeResult);
    Task<QuizzeResult?> UpdateAsync(QuizzeResult quizzeResult);
    Task<bool> DeleteAsync(Guid id);
      Task<bool> ExistsAsync(Guid id);
    }
}
