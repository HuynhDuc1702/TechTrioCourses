using QuizAPI.Application.DTOs.Request.Question;
using QuizAPI.Application.DTOs.Response.Question;

namespace QuizAPI.Application.Interfaces.IServices
{
    public interface IQuestionService
    {
        Task<IEnumerable<QuestionResponse>> GetAllQuestionsAsync();
        Task<QuestionResponse?> GetQuestionByIdAsync(Guid id);
        Task<IEnumerable<QuestionResponse>> GetQuestionCourseIdAsync(Guid courseId);
        Task<QuestionResponse> CreateQuestionAsync(CreateQuestionRequest request);
        Task<QuestionResponse?> UpdateQuestionAsync(Guid id, UpdateQuestionRequest request);

        Task<bool> DeleteQuestionAsync(Guid id);
        Task<bool> DisableQuestionAsync(Guid id);
        Task<bool> ArchiveQuestionAsync(Guid id);
    }
}
