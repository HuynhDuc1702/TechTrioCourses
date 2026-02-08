using QuizAPI.Domain.Entities;

namespace QuizAPI.Application.Interfaces.IRepositories
{
    public interface IQuestionChoiceRepository
    {
        Task<IEnumerable<QuestionChoice>> GetAllAsync();
        Task<QuestionChoice?> GetByIdAsync(Guid id);
        Task<IEnumerable<QuestionChoice>> GetByQuestionIdAsync(Guid questionId);
        
        Task<QuestionChoice> CreateAsync(QuestionChoice questionChoice);
        Task<QuestionChoice?> UpdateAsync(QuestionChoice questionChoice);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
    }
}
