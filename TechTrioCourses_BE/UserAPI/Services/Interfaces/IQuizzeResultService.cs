using UserAPI.DTOs.Request.QuizzeResult;
using UserAPI.DTOs.Response.QuizzeResult;

namespace UserAPI.Services.Interfaces
{
    public interface IQuizzeResultService
  {
     Task<IEnumerable<QuizzeResultResponse>> GetAllQuizzeResultsAsync();
        Task<QuizzeResultResponse?> GetQuizzeResultByIdAsync(Guid id);
        Task<IEnumerable<QuizzeResultResponse>> GetQuizzeResultsByUserIdAsync(Guid userId);
        Task<IEnumerable<QuizzeResultResponse>> GetQuizzeResultsByQuizIdAsync(Guid quizId);
        Task<IEnumerable<QuizzeResultResponse>> GetQuizzeResultsByUserAndQuizIdAsync(Guid userId, Guid quizId);
        Task<QuizzeResultResponse> CreateQuizzeResultAsync(CreateQuizzeResultRequest request);
        Task<QuizzeResultResponse?> UpdateQuizzeResultAsync(Guid id, UpdateQuizzeResultRequest request);
      Task<bool> DeleteQuizzeResultAsync(Guid id);
    }
}
