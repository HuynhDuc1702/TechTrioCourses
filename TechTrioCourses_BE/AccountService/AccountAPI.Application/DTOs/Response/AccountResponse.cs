using TechTrioCourses.Shared.Enums;

namespace AccountAPI.Application.DTOs.Response
{
    public class AccountResponse
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        public string Email { get; set; }

        public string FullName { get; set; }
        public AccountStatusEnum Status { get; set; }
        public string? AvatarUrl { get; set; }
        public UserRoleEnum Role { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
