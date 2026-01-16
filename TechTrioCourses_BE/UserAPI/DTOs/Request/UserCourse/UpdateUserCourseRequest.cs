using TechTrioCourses.Shared.Enums;

namespace UserAPI.DTOs.Request.UserCourse
{
    public class UpdateUserCourseRequest
    {
        public UserCourseStatusEnum? Status { get; set; }

        public DateTime? CompletedAt { get; set; }
    }
}
