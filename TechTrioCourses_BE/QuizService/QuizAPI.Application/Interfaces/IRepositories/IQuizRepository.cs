using QuizAPI.Domain.Entities;

namespace QuizAPI.Application.Interfaces.IRepositories
{
    public interface IQuizRepository
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
