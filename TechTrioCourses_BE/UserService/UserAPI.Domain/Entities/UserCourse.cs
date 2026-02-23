
using TechTrioCourses.Shared.Abstractions;
using TechTrioCourses.Shared.Enums;

namespace UserAPI.Domain.Entities;

public partial class UserCourse : BaseEntity
{

    public Guid UserId { get; set; }

    public Guid CourseId { get; set; }

  public UserCourseStatusEnum Status { get; set; }

    public double Progress { get; set; }

    public DateTime EnrolledAt { get; set; }

    public DateTime? CompletedAt { get; set; }

}
