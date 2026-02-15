using QuizAPI.Application.DTOs.Request.GradeQuizDTOs;
using QuizAPI.Application.DTOs.Request.QuestionChoice;
using QuizAPI.Application.DTOs.Response.QuestionChoice;

namespace QuizAPI.Application.Interfaces.IServices
{
    public interface IQuestionChoiceService
    {
        Task<IEnumerable<QuestionChoiceResponse>> GetAllQuestionChoicesAsync();
        Task<QuestionChoiceResponse?> GetQuestionChoiceByIdAsync(Guid id);
        Task<IEnumerable<QuestionChoiceResponse>> GetQuestionChoicesByQuestionIdAsync(Guid questionId);
       
        Task<bool> GradeMultipleQuestion(UserQuestionAnswersDtos userAnswers);
        Task<bool> GradeTrueFalseQuestion(UserQuestionAnswersDtos userAnswers);
        Task<QuestionChoiceResponse> CreateQuestionChoiceAsync(CreateQuestionChoiceRequest request);
        Task<QuestionChoiceResponse?> UpdateQuestionChoiceAsync(Guid id, UpdateQuestionChoiceRequest request);
        Task<bool> DeleteQuestionChoiceAsync(Guid id);
    }
}
