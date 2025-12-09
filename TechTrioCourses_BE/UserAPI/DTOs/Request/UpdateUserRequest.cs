using UserAPI.Enums;

namespace UserAPI.DTOs.Request
{
    public class UpdateUserRequest
    {
        public string? FullName { get; set; }

        public string? AvatarUrl { get; set; }

        public UserRoleEnum? Role { get; set; }
    }
}
