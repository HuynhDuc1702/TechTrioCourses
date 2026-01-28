using QuizAPI.Models;

namespace QuizAPI.Repositories.Interfaces
{
    public interface IQuizRepo
    {
        Task<IEnumerable<Quiz>> GetAllAsync();
        Task<Quiz?> GetByIdAsync(Guid id);
        Task<IEnumerable<Quiz>> GetByCourseIdAsync(Guid courseId);
        Task<Quiz> CreateAsync(Quiz quiz);
        Task<Quiz?> UpdateAsync(Quiz quiz); 
        Task<bool> DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
    }
}
