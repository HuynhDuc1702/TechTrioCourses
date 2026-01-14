using System;
using System.Collections.Generic;
using TechTrioCourses.Shared.Enums;

namespace UserAPI.Models;

public partial class UserLesson
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public Guid LessonId { get; set; }
    public Guid? CourseId { get; set; }

    public UserLessonStatusEnum Status { get; set; }

    public DateTime? CompletedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}
