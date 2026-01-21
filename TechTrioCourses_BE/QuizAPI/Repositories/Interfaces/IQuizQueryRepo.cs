namespace QuizAPI.Repositories.Interfaces
{
    using QuizAPI.DTOs.Projections.AttemptQuizDetailProjections;
    using QuizAPI.DTOs.Projections.FullQuizDetailProjections;

    public interface IQuizQueryRepo
    {
        Task<AttemptQuizDetailResponseProjection?> GetQuizDetailForAttemptAsync(Guid quizId);
        Task<QuizDetailProjection?> GetQuizDetailAsync(Guid quizId);
    }
}
