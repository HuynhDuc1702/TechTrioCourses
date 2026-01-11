using TechTrioCourses.Shared.Enums;

namespace UserAPI.DTOs.Response.UserLesson
{
    public class UserLessonResponse
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
   public Guid LessonId { get; set; }
  public UserLessonStatusEnum Status { get; set; }
  public DateTime? CompletedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
