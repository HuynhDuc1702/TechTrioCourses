using AccountAPI.Application.DTOs.Response;
using System.Security.Claims;
using TechTrioCourses.Shared.Dtos.User;

namespace AccountAPI.Application.Interfaces.ISecurity
{
    public interface IJwtTokenService
    {
        string GenerateAccessToken(AccountResponse account, UserResponseForLogin? user, string jwtId);
        string GenerateRefreshToken(Guid accountId, string jwtId);
     ClaimsPrincipal? ValidateRefreshToken(string refreshToken);
    }
}
