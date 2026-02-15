using UserAPI.Application.DTOs.Request.UserQuiz;
using UserAPI.Application.DTOs.Response.UserQuiz;


namespace UserAPI.Application.Interfaces.IServices
{
    public interface IUserQuizService
    {
        Task<UserQuizResponse?> GetUserQuizByIdAsync(Guid id);
        Task<IEnumerable<UserQuizResponse>> GetAllUserQuizzesAsync();
        Task<IEnumerable<UserQuizResponse>> GetUserQuizzesByUserIdAsync(Guid userId);
        Task<IEnumerable<UserQuizResponse>> GetUserQuizzesByQuizIdAsync(Guid quizId);
        Task<IEnumerable<UserQuizResponse>> GetUserQuizzesByCourseIdAsync(Guid courseId);
        Task<IEnumerable<UserQuizResponse>> GetUserQuizzesByUserAndCourseAsync(Guid userId, Guid courseId);
        Task<UserQuizResponse?> GetUserQuizByUserAndQuizAsync(Guid userId, Guid quizId);
       
        Task<UserQuizResponse?> CreateUserQuizAsync(CreateUserQuizRequest request);
        Task<UserQuizResponse?> UpdateUserQuizAsync(Guid id, ApplyQuizGradingResultRequest request);
        Task<UserQuizResponse?> RetakeUserQuizAsync(Guid id);
        Task<bool> DeleteUserQuizAsync(Guid id);
    }
}
