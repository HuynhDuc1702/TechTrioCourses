using UserAPI.Enums;

namespace UserAPI.DTOs.Request.User
{
    public class UpdateUserRequest
    {
  public string? FullName { get; set; }

     public string? AvatarUrl { get; set; }

 public UserRoleEnum? Role { get; set; }
    }
}
