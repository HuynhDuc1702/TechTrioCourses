using UserAPI.DTOs.Request;
using UserAPI.DTOs.Response;

namespace UserAPI.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserResponse?> GetUserByIdAsync(Guid id);
        Task<UserResponse?> GetUserByAccountIdAsync(Guid accountId);
        Task<IEnumerable<UserResponse>> GetUsersByIdsAsync(List<Guid> ids);
        Task<UserResponse?> CreateUserAsync(CreateUserRequest request);
        Task<UserResponse?> UpdateUserAsync(Guid id, UpdateUserRequest request);
        Task<IEnumerable<UserResponse?>> GetAllUsersAsync();
    }
}
