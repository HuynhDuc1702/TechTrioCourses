using LessonAPI.Enums;

namespace LessonAPI.DTOs.Request
{
    public class CreateLessonRequest
    {
        public Guid CourseId { get; set; }

        public string Title { get; set; } = null!;

        public string? Content { get; set; }

        public string? MediaUrl { get; set; }

        public string? MediaType { get; set; }

        public int? OrderIndex { get; set; }
        public LessonStatusEnum? Status { get; set; }

    }
}
