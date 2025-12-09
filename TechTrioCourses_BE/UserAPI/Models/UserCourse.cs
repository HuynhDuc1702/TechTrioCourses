using System;
using System.Collections.Generic;
using UserAPI.Enums;

namespace UserAPI.Models;

public partial class UserCourse
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public Guid CourseId { get; set; }

    public UserCourseStatus Status { get; set; }

    public double Progress { get; set; }

    public DateTime EnrolledAt { get; set; }

    public DateTime? CompletedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}
