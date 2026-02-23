
using TechTrioCourses.Shared.Abstractions;
using TechTrioCourses.Shared.Enums;

namespace AccountAPI.Domain.Entities;

public partial class Account : BaseEntity
{
   

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public AccountStatusEnum Status { get; set; }

    
}
