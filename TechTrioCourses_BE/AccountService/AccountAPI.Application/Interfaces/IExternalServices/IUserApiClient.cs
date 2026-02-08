

using AccountAPI.Application.DTOs.Request;
using TechTrioCourses.Shared.Dtos.User;
using TechTrioCourses.Shared.Enums;

namespace AccountAPI.Application.Interfaces.IExternalServices
{
    public interface IUserApiClient
    {
       Task<UserResponseForRegister?> RegisterUser(Guid accountId, RegisterRequest request);
        Task<UserResponseForLogin> GetUserByAccountIdFromUserAPI(Guid accountId);
        public string ConvertRoleToEnumName(UserRoleEnum role);
    }
}
