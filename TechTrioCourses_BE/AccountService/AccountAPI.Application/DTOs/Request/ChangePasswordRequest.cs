namespace AccountAPI.Application.DTOs.Request
{
    public class ChangePasswordRequest
    {
        public required string Email { get; set; }
        public required string OldPassword { get; set; }
    public required string NewPassword { get; set; }
 }
}
