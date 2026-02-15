using QuizAPI.Domain.Entities;

namespace QuizAPI.Application.Interfaces.IRepositories
{
    public interface IQuestionAnswerRepository
    {
        Task<IEnumerable<QuestionAnswer>> GetAllAsync();
        Task<QuestionAnswer?> GetByIdAsync(Guid id);
        Task<IEnumerable<QuestionAnswer>> GetByQuestionIdAsync(Guid questionId);
        Task<QuestionAnswer> CreateAsync(QuestionAnswer questionAnswer);
        Task<QuestionAnswer?> UpdateAsync(QuestionAnswer questionAnswer);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
    }
}
