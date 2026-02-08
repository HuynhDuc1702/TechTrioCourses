
using TechTrioCourses.Shared.Enums;

namespace TechTrioCourses.Shared.Dtos.User
{
    public class UserResponseForLogin
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public string FullName { get; set; } = null!;
        public string? AvatarUrl { get; set; }
        public UserRoleEnum Role { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
