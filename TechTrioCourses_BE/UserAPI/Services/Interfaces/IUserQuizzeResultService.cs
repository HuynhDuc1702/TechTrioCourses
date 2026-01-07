using UserAPI.DTOs.Request.UserQuizzeResult;
using UserAPI.DTOs.Response.UserQuizzeResult;

namespace UserAPI.Services.Interfaces
{
    public interface IUserQuizzeResultService
  {
     Task<IEnumerable<UserQuizzeResultResponse>> GetAllQuizzeResultsAsync();
        Task<UserQuizzeResultResponse?> GetQuizzeResultByIdAsync(Guid id);
        Task<IEnumerable<UserQuizzeResultResponse>> GetQuizzeResultsByUserIdAsync(Guid userId);
        Task<IEnumerable<UserQuizzeResultResponse>> GetQuizzeResultsByQuizIdAsync(Guid quizId);
        Task<IEnumerable<UserQuizzeResultResponse>> GetQuizzeResultsByUserAndQuizIdAsync(Guid userId, Guid quizId);
        Task<UserQuizzeResultResponse> CreateQuizzeResultAsync(CreateUserQuizzeResultRequest request);
        Task<UserQuizzeResultResponse?> UpdateQuizzeResultAsync(Guid id, UpdateUserQuizzeResultRequest request);
      Task<bool> DeleteQuizzeResultAsync(Guid id);
    }
}
