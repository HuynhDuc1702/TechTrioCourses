using UserAPI.Application.DTOs.Projections.AttemptUserQuizzeResultDetailProjections;

namespace UserAPI.Application.Interfaces.IRepositories
{
    public interface IUserQuizzeResultQueryRepository
    {
        Task<UserQuizzeResultDetailResponseProjection?> GetUserQuizzeResultDetailAsync(Guid id);
    }
}
