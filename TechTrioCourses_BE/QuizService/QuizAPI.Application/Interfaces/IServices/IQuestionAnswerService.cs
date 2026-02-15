using QuizAPI.Application.DTOs.Request.GradeQuizDTOs;
using QuizAPI.Application.DTOs.Request.QuestionAnswer;
using QuizAPI.Application.DTOs.Response.QuestionAnswer;

namespace QuizAPI.Application.Interfaces.IServices
{
    public interface IQuestionAnswerService
    {
        Task<IEnumerable<QuestionAnswerResponse>> GetAllQuestionAnswersAsync();
        Task<QuestionAnswerResponse?> GetQuestionAnswerByIdAsync(Guid id);
        Task<IEnumerable<QuestionAnswerResponse>> GetQuestionAnswersByQuestionIdAsync(Guid questionId);
        Task<QuestionAnswerResponse> CreateQuestionAnswerAsync(CreateQuestionAnswerRequest request);
        Task<bool> GradeShortAnswer(UserQuestionAnswersDtos userAnswers);
        Task<QuestionAnswerResponse?> UpdateQuestionAnswerAsync(Guid id, UpdateQuestionAnswerRequest request);
        Task<bool> DeleteQuestionAnswerAsync(Guid id);
    }
}
