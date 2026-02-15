namespace QuizAPI.Application.Interfaces.IRepositories
{
    using QuizAPI.Application.DTOs.Projections.AttemptQuizDetailProjections;
    using QuizAPI.Application.DTOs.Projections.QuizDetailProjections;

    public interface IQuizQueryRepository
    {
        Task<AttemptQuizDetailResponseProjection?> GetQuizDetailForAttemptAsync(Guid quizId);
        Task<QuizDetailProjection?> GetQuizDetailAsync(Guid quizId);
    }
}
