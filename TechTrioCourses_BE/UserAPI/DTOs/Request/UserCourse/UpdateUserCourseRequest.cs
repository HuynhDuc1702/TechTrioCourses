using UserAPI.Enums;

namespace UserAPI.DTOs.Request.UserCourse
{
    public class UpdateUserCourseRequest
    {
        public UserCourseStatus? Status { get; set; }

        public DateTime? CompletedAt { get; set; }
    }
}
