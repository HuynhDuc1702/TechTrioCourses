using UserAPI.Enums;

namespace UserAPI.DTOs.Response
{
    public class UserResponse
    {
        public Guid Id { get; set; }

        public Guid AccountId { get; set; }

        public string FullName { get; set; } = null!;

        public string? AvatarUrl { get; set; }

        public UserRoleEnum Role { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
