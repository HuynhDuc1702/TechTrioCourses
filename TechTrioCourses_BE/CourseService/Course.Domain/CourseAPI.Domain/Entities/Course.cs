using TechTrioCourses.Shared.Enums;
using System;
using System.Collections.Generic;
using TechTrioCourses.Shared.Abstractions;

namespace CourseAPI.Domain.Entities;

public partial class Course : BaseEntity
{

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public Guid? CategoryId { get; set; }

    public Guid? CreatorId { get; set; }

    public PublishStatusEnum Status { get; set; }

}
