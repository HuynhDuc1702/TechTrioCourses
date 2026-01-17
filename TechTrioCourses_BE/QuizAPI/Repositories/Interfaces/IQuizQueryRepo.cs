namespace QuizAPI.Repositories.Interfaces
{
    using QuizAPI.DTOs.Projections.AttemptQuizDetailProjections;

    public interface IQuizQuery
    {
        Task<QuizDetailResponseProjection?> GetQuizDetailForAttemptAsync(Guid quizId);
    }
}
