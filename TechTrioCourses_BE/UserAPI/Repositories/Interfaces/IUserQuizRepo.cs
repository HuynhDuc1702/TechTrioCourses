using TechTrioCourses.Shared.Enums;
using UserAPI.Models;

namespace UserAPI.Repositories.Interfaces
{
    public interface IUserQuizRepo
    {
        Task<UserQuiz?> GetByIdAsync(Guid id);
        Task<IEnumerable<UserQuiz>> GetAllAsync();
        Task<IEnumerable<UserQuiz>> GetByUserIdAsync(Guid userId);
        Task<IEnumerable<UserQuiz>> GetByQuizIdAsync(Guid quizId);
        Task<IEnumerable<UserQuiz>> GetByCourseIdAsync(Guid courseId);
        Task<IEnumerable<UserQuiz>> GetByUserAndCourseAsync(Guid userId, Guid courseId);
        Task<UserQuiz?> GetByUserAndQuizAsync(Guid userId, Guid quizId);
        Task<UserQuiz> CreateUserQuizAsync(UserQuiz userQuiz);
        Task<bool> UpdateUserQuizAsync(Guid userId, Guid quizId, UserQuizStatusEnum newStatus, double? score);
        Task<bool> DeleteUserQuizAsync(Guid id);
        Task<bool> UserQuizExistsAsync(Guid userId, Guid quizId);
    }
}
