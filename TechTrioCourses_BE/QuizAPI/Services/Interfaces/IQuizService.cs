using QuizAPI.DTOs.Request.Quiz;
using QuizAPI.DTOs.Response.Quiz;

namespace QuizAPI.Services.Interfaces
{
    public interface IQuizService
    {
        Task<IEnumerable<QuizResponse>> GetAllQuizzesAsync();
        Task<QuizResponse?> GetQuizByIdAsync(Guid id);
        Task<QuizResponse> CreateQuizAsync(CreateQuizRequest request);
        Task<QuizResponse?> UpdateQuizAsync(Guid id, UpdateQuizRequest request);
        Task<bool> DeleteQuizAsync(Guid id);
        Task<bool> DisableQuizAsync(Guid id);
        Task<bool> ArchiveQuizAsync(Guid id);
    }
}
