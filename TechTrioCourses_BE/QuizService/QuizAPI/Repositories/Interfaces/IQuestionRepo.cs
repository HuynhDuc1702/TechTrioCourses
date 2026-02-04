using QuizAPI.Models;

namespace QuizAPI.Repositories.Interfaces
{
    public interface IQuestionRepo
    {
        Task<IEnumerable<Question>> GetAllAsync();
        Task<Question?> GetByIdAsync(Guid id);
        Task<Question> CreateAsync(Question question);
        Task<Question?> UpdateAsync(Question question);
        Task<IEnumerable<Question>> GetQuestionsByCourseId(Guid courseId);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
    }
}
