using QuizAPI.DTOs.Request.Question;
using QuizAPI.DTOs.Response.Question;

namespace QuizAPI.Services.Interfaces
{
    public interface IQuestionService
    {
        Task<IEnumerable<QuestionResponse>> GetAllQuestionsAsync();
        Task<QuestionResponse?> GetQuestionByIdAsync(Guid id);
        Task<QuestionResponse> CreateQuestionAsync(CreateQuestionRequest request);
        Task<QuestionResponse?> UpdateQuestionAsync(Guid id, UpdateQuestionRequest request);
        Task<bool> DeleteQuestionAsync(Guid id);
        Task<bool> DisableQuestionAsync(Guid id);
        Task<bool> ArchiveQuestionAsync(Guid id);
    }
}
