using TechTrioCourses.Shared.Enums;

namespace CourseAPI.DTOs.Request
{
    public class CreateCourseRequest
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid? CreatorId { get; set; }
        public PublishStatusEnum Status { get; set; }
    }
}
    