using UserAPI.Enums;

namespace UserAPI.DTOs.Response
{
    public class UserLessonResponse
    {
        public Guid Id { get; set; }
   public Guid UserId { get; set; }
        public Guid LessonId { get; set; }
        public UserLessonStatus Status { get; set; }
        public DateTime? CompletedAt { get; set; }
     public DateTime UpdatedAt { get; set; }
    }
}
