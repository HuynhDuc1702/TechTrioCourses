using AccountAPI.Models;
using AccountAPI.Enums;

namespace AccountAPI.DTOs.Response
{
    public class AuthResult
    {
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
        public DateTime AccessTokenExpiresAt { get; set; }
        public DateTime RefreshTokenExpiresAt { get; set; }
    }
}
