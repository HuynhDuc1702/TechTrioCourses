namespace AccountAPI.DTOs.Request
{
    public class VerifyOtpRequest
  {
        public required string Email { get; set; }
     public required string Code { get; set; }
    }
}
