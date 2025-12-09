using QuizAPI.DTOs.Request.QuestionAnswer;
using QuizAPI.DTOs.Response.QuestionAnswer;

namespace QuizAPI.Services.Interfaces
{
    public interface IQuestionAnswerService
 {
   Task<IEnumerable<QuestionAnswerResponse>> GetAllQuestionAnswersAsync();
        Task<QuestionAnswerResponse?> GetQuestionAnswerByIdAsync(Guid id);
 Task<IEnumerable<QuestionAnswerResponse>> GetQuestionAnswersByResultIdAsync(Guid resultId);
      Task<QuestionAnswerResponse> CreateQuestionAnswerAsync(CreateQuestionAnswerRequest request);
        Task<QuestionAnswerResponse?> UpdateQuestionAnswerAsync(Guid id, UpdateQuestionAnswerRequest request);
        Task<bool> DeleteQuestionAnswerAsync(Guid id);
    }
}
