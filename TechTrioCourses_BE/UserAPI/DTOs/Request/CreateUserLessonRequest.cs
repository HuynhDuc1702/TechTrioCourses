using UserAPI.Enums;

namespace UserAPI.DTOs.Request
{
    public class CreateUserLessonRequest
    {
        public Guid UserId { get; set; }
        public Guid LessonId { get; set; }
 
 }
}
