using TechTrioCourses.Shared.Enums;

namespace UserAPI.Application.DTOs.Request.User
{
    public class CreateUserRequest
    {
        public Guid AccountId { get; set; }
        public string FullName { get; set; } = null!;
        public string? AvatarUrl { get; set; }
        public UserRoleEnum Role { get; set; } = UserRoleEnum.Student;
    }
}
