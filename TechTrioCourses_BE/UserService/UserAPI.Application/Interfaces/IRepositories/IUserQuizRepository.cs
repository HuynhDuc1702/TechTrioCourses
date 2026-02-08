using UserAPI.Domain.Entities;


namespace UserAPI.Application.Interfaces.IRepositories
{
    public interface IUserQuizRepository
    {
        Task<UserQuiz?> GetByIdAsync(Guid id);
        Task<IEnumerable<UserQuiz>> GetAllAsync();
        Task<IEnumerable<UserQuiz>> GetByUserIdAsync(Guid userId);
        Task<IEnumerable<UserQuiz>> GetByQuizIdAsync(Guid quizId);
        Task<IEnumerable<UserQuiz>> GetByCourseIdAsync(Guid courseId);
        Task<IEnumerable<UserQuiz>> GetByUserAndCourseAsync(Guid userId, Guid courseId);
        Task<UserQuiz?> GetByUserAndQuizAsync(Guid userId, Guid quizId);
        Task<UserQuiz> CreateUserQuizAsync(UserQuiz userQuiz);
        Task<bool> UpdateUserQuizAsync(UserQuiz userQuiz);
        Task<bool> DeleteUserQuizAsync(Guid id);
        Task<bool> UserQuizExistsAsync(Guid userId, Guid quizId);
    }
}
