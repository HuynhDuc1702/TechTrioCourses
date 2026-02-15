using UserAPI.Application.DTOs.Request.SubmitQuizDTOs;
using UserAPI.Application.DTOs.Request.UserQuizzeResult;
using UserAPI.Application.DTOs.Response.AttemptUserQuizzeResultDetailDTOs;
using UserAPI.Application.DTOs.Response.UserQuizzeResult;

namespace UserAPI.Application.Interfaces.IServices
{
    public interface IUserQuizzeResultService
    {
        Task<IEnumerable<UserQuizzeResultResponse>> GetAllQuizzeResultsAsync();
        Task<UserQuizzeResultResponse?> GetQuizzeResultByIdAsync(Guid id);
        Task<UserQuizzeResultReviewResponseDtos?> GetUserQuizzeResultDetailForAttemptReviewAsync(Guid id);
        Task<UserQuizzeResultResumeResponseDto?> GetUserQuizzeResultDetailForAttemptResumeAsync(Guid id);
        Task<IEnumerable<UserQuizzeResultResponse>> GetQuizzeResultsByUserIdAsync(Guid userId);
        Task<IEnumerable<UserQuizzeResultResponse>> GetQuizzeResultsByQuizIdAsync(Guid quizId);
        Task<IEnumerable<UserQuizzeResultResponse>> GetQuizzeResultsByUserAndQuizIdAsync(Guid userId, Guid quizId);
        Task<IEnumerable<UserQuizzeResultResponse>> GetQuizzeResultsByUserQuizIdAsync(Guid userQuizId);
        Task<UserQuizzeResultResponse> GetLatestUserQuizzeResult(Guid userQuizId);
        Task<UserQuizzeResultResponse> CreateQuizzeResultAsync(CreateUserQuizzeResultRequest request);
        Task SaveUserAnswersAsync(Guid resultId, List<UserQuestionAnswersDtos> answers);
        Task<SubmitQuizResponseDto?> SubmitQuizAsync(SubmitQuizRequestDto request);
        Task<UserQuizzeResultResponse?> UpdateQuizzeResultAsync(Guid id, UpdateUserQuizzeResultRequest request);
        Task<bool> DeleteQuizzeResultAsync(Guid id);
    }
}
