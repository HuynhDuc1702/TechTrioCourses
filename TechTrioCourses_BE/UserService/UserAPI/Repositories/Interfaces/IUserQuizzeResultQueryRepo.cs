using UserAPI.DTOs.Projections.AttemptUserQuizzeResultDetailProjections;

namespace UserAPI.Repositories.Interfaces
{
    public interface IUserQuizzeResultQueryRepo
    {
        Task<UserQuizzeResultDetailResponseProjection?> GetUserQuizzeResultDetailAsync(Guid id);
    }
}
