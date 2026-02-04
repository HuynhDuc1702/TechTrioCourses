using QuizAPI.DTOs.Request.GradeQuizDTOs;
using QuizAPI.DTOs.Request.Quiz;
using QuizAPI.DTOs.Response.AttemptQuizDetailDTOs;
using QuizAPI.DTOs.Response.FullQuizDetailDTOs;
using QuizAPI.DTOs.Response.Quiz;

namespace QuizAPI.Services.Interfaces
{
    public interface IQuizService
    {
        Task<IEnumerable<QuizResponse>> GetAllQuizzesAsync();
        Task<QuizResponse?> GetQuizByIdAsync(Guid id);
        Task<IEnumerable<QuizResponse>> GetQuizzesByCourseIdAsync(Guid courseId);
        Task<AttemptQuizDetailResponseDto?> GetQuizDetailForAttemptAsync(Guid quizId);
        Task<QuizDetailResponseDto?> GetQuizDetailAsync(Guid quizId);
        Task<GradingResultDto> GradeQuizAsync(GradeQuizRequestDto request);
        Task<QuizResponse> CreateQuizAsync(CreateQuizRequest request);
        Task<QuizResponse?> UpdateQuizAsync(Guid id, UpdateQuizRequest request);
        Task<bool> DeleteQuizAsync(Guid id);
        Task<bool> DisableQuizAsync(Guid id);
        Task<bool> ArchiveQuizAsync(Guid id);
    }
}
