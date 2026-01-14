using TechTrioCourses.Shared.Enums;

namespace UserAPI.DTOs.Request.UserLesson
{
    public class UpdateUserLessonRequest
    {
        public UserLessonStatusEnum? Status { get; set; }
        public DateTime? CompletedAt { get; set; }
    }
}
