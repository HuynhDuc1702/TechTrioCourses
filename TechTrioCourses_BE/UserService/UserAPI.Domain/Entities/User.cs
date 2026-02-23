
using TechTrioCourses.Shared.Abstractions;
using TechTrioCourses.Shared.Enums;

namespace UserAPI.Domain.Entities;

public partial class User : BaseEntity
{


    public Guid AccountId { get; set; }

    public string FullName { get; set; } = null!;

    public string? AvatarUrl { get; set; }

    public UserRoleEnum Role { get; set; }

  
}
