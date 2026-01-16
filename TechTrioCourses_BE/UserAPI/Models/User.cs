using System;
using System.Collections.Generic;
using TechTrioCourses.Shared.Enums;

namespace UserAPI.Models;

public partial class User
{
  public Guid Id { get; set; }

    public Guid AccountId { get; set; }

    public string FullName { get; set; } = null!;

    public string? AvatarUrl { get; set; }

    public UserRoleEnum Role { get; set; }

    public DateTime? CreatedAt { get; set; }
}
