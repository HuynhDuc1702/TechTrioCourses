using UserAPI.Enums;

namespace UserAPI.DTOs.Request
{
    public class CreateUserCourseRequest
    {
        public Guid UserId { get; set; }
        public Guid CourseId { get; set; }
        public UserCourseStatus Status { get; set; }
    public double Progress { get; set; }
    }
}
