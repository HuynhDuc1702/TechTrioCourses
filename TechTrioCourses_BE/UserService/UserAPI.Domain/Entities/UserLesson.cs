
using TechTrioCourses.Shared.Abstractions;
using TechTrioCourses.Shared.Enums;

namespace UserAPI.Domain.Entities;

public partial class UserLesson : BaseEntity
{

    public Guid UserId { get; set; }

    public Guid LessonId { get; set; }
    public Guid? CourseId { get; set; }

    public UserLessonStatusEnum Status { get; set; }

    public DateTime? CompletedAt { get; set; }

 
}
