using AccountAPI.Enums;

namespace AccountAPI.DTOs.Request
{
    public class RegisterRequest
    {
        // Account fields
        public string Email { get; set; }
        public string Password { get; set; }

        // User fields
        public string FullName { get; set; }
        public string? AvatarUrl { get; set; }
        
    }
}
