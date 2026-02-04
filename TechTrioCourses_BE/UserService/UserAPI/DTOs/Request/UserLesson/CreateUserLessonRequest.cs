namespace UserAPI.DTOs.Request.UserLesson
{
    public class CreateUserLessonRequest
    {
        public Guid UserId { get; set; }
  public Guid LessonId { get; set; }
   public Guid CourseId { get; set; }
    }
}
