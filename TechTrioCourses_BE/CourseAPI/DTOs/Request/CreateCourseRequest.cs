using CourseAPI.Enums;

namespace CourseAPI.DTOs.Request
{
    public class CreateCourseRequest
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid? CreatorId { get; set; }
        public CourseStatusEnum Status { get; set; }
    }
}
    