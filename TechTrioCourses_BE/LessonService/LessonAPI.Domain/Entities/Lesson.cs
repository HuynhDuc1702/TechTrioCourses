using TechTrioCourses.Shared.Enums;
using TechTrioCourses.Shared.Abstractions;

namespace LessonAPI.Domain.Entities;

public partial class Lesson : BaseEntity
{
   
    public Guid CourseId { get; set; }

    public string Title { get; set; } = null!;

    public string? Content { get; set; }

    public string? MediaUrl { get; set; }

    public LessonMediaTypeEnum MediaType { get; set; }

    public int? OrderIndex { get; set; }

    public PublishStatusEnum? Status { get; set; }
}
