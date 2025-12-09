using QuizAPI.DTOs.Request.QuizzeResult;
using QuizAPI.DTOs.Response.QuizzeResult;

namespace QuizAPI.Services.Interfaces
{
  public interface IQuizzeResultService
  {
    Task<IEnumerable<QuizzeResultResponse>> GetAllQuizzeResultsAsync();
        Task<QuizzeResultResponse?> GetQuizzeResultByIdAsync(Guid id);
        Task<QuizzeResultResponse> CreateQuizzeResultAsync(CreateQuizzeResultRequest request);
   Task<QuizzeResultResponse?> UpdateQuizzeResultAsync(Guid id, UpdateQuizzeResultRequest request);
     Task<bool> DeleteQuizzeResultAsync(Guid id);
}
}
