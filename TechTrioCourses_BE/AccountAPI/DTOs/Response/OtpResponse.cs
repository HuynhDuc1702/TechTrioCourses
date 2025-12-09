namespace AccountAPI.DTOs.Response
{
    public class OtpResponse
  {
        public required string Message { get; set; }
        public DateTime? ExpiresAt { get; set; }
    }
}
