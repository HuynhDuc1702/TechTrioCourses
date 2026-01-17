using UserAPI.DTOs.Request.UserQuiz;
using UserAPI.DTOs.Response.AttemptUserQuizzeResultDetailDTOs;
using UserAPI.DTOs.Response.UserQuiz;

namespace UserAPI.Services.Interfaces
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
        Task<UserQuizResponse?> UpdateUserQuizAsync(Guid id, SubmitUserQuizRequest request);
        Task<UserQuizResponse?> RetakeUserQuizAsync(Guid id);
        Task<bool> DeleteUserQuizAsync(Guid id);
    }
}
