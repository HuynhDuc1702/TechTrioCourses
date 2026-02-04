using QuizAPI.Models;

namespace QuizAPI.Repositories.Interfaces
{
    public interface IQuizQuestionRepo
    {
        Task<IEnumerable<QuizQuestion>> GetAllAsync();
        Task<QuizQuestion?> GetByIdAsync(Guid quizId, Guid questionId);
        Task<IEnumerable<QuizQuestion>> GetByQuizIdAsync(Guid quizId);
        Task<IEnumerable<QuizQuestion>> GetByQuestionIdAsync(Guid questionId);

        Task<QuizQuestion> CreateAsync(QuizQuestion quizQuestion);
        Task<QuizQuestion?> UpdateAsync(QuizQuestion quizQuestion);
        Task<bool> DeleteAsync(Guid quizId, Guid questionId);
        Task<bool> ExistsAsync(Guid quizId, Guid questionId);
    }
}
