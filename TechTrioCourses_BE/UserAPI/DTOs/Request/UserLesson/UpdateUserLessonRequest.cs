using UserAPI.Enums;

namespace UserAPI.DTOs.Request.UserLesson
{
    public class UpdateUserLessonRequest
    {
        public UserLessonStatus? Status { get; set; }
        public DateTime? CompletedAt { get; set; }
    }
}
