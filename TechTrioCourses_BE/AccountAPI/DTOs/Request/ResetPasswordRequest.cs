namespace AccountAPI.DTOs.Request
{
    public class ResetPasswordRequest
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}


