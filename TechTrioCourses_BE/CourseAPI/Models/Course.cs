using CourseAPI.Enums;
using System;
using System.Collections.Generic;

namespace CourseAPI.Models;

public partial class Course
{
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public Guid? CategoryId { get; set; }

    public Guid? CreatorId { get; set; }

    public CourseStatusEnum Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
