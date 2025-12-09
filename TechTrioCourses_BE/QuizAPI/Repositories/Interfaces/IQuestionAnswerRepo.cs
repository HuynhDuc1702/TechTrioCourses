using QuizAPI.Models;

namespace QuizAPI.Repositories.Interfaces
{
    public interface IQuestionAnswerRepo
    {
        Task<IEnumerable<QuestionAnswer>> GetAllAsync();
 Task<QuestionAnswer?> GetByIdAsync(Guid id);
        Task<IEnumerable<QuestionAnswer>> GetByResultIdAsync(Guid resultId);
      Task<QuestionAnswer> CreateAsync(QuestionAnswer questionAnswer);
        Task<QuestionAnswer?> UpdateAsync(QuestionAnswer questionAnswer);
   Task<bool> DeleteAsync(Guid id);
   Task<bool> ExistsAsync(Guid id);
    }
}
