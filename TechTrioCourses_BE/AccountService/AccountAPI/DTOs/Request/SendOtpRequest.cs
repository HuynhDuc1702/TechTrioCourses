namespace AccountAPI.DTOs.Request
{
    public class SendOtpRequest
    {
  public required string Email { get; set; }
        public required string Purpose { get; set; } // "Registration", "PasswordReset", "EmailVerification"
    }
}
