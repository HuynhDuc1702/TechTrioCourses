using AccountAPI.Models;


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
