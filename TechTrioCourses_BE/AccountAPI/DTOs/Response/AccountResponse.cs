namespace AccountAPI.DTOs.Response
{
    public class AccountResponse
    {
        public Guid AccountId { get; set; }
        public Guid UserId { get; set; }

        public string Email { get; set; }

        public string FullName { get; set; }
        public string? AvatarUrl { get; set; }
        public short Role { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
