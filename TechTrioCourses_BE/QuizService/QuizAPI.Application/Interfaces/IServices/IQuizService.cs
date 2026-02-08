using QuizAPI.Application.DTOs.Request.GradeQuizDTOs;
using QuizAPI.Application.DTOs.Request.Quiz;
using QuizAPI.Application.DTOs.Response.AttemptQuizDetailDTOs;
using QuizAPI.Application.DTOs.Response.Quiz;
using QuizAPI.Application.DTOs.Response.QuizDetailDTOs;

namespace QuizAPI.Application.Interfaces.IServices
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
