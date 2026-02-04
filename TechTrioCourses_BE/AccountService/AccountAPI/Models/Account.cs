
using System;
using TechTrioCourses.Shared.Enums;
using System.Collections.Generic;

namespace AccountAPI.Models;

public partial class Account
{
    public Guid Id { get; set; }

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public AccountStatusEnum Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
