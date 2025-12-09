using QuizAPI.DTOs.Request.QuestionChoice;
using QuizAPI.DTOs.Response.QuestionChoice;

namespace QuizAPI.Services.Interfaces
{
 public interface IQuestionChoiceService
    {
        Task<IEnumerable<QuestionChoiceResponse>> GetAllQuestionChoicesAsync();
    Task<QuestionChoiceResponse?> GetQuestionChoiceByIdAsync(Guid id);
  Task<IEnumerable<QuestionChoiceResponse>> GetQuestionChoicesByQuestionIdAsync(Guid questionId);
   Task<QuestionChoiceResponse> CreateQuestionChoiceAsync(CreateQuestionChoiceRequest request);
   Task<QuestionChoiceResponse?> UpdateQuestionChoiceAsync(Guid id, UpdateQuestionChoiceRequest request);
     Task<bool> DeleteQuestionChoiceAsync(Guid id);
  }
}
