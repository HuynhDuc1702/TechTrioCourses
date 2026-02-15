using UserAPI.Application.DTOs.Request.User;
using UserAPI.Application.DTOs.Response.User;

namespace UserAPI.Application.Interfaces.IServices
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
