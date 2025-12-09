using QuizAPI.Models;

namespace QuizAPI.Repositories.Interfaces
{
  public interface IQuizzeResultRepo
 {
        Task<IEnumerable<QuizzeResult>> GetAllAsync();
  Task<QuizzeResult?> GetByIdAsync(Guid id);
        Task<QuizzeResult> CreateAsync(QuizzeResult quizzeResult);
        Task<QuizzeResult?> UpdateAsync(QuizzeResult quizzeResult);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
 }
}
