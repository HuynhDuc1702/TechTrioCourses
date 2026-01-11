using TechTrioCourses.Shared.Enums;
namespace CourseAPI.DTOs.Request
{
    public class UpdateCourseRequest
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public Guid? CategoryId { get; set; }
        public PublishStatusEnum? Status { get; set; }
    }
}
