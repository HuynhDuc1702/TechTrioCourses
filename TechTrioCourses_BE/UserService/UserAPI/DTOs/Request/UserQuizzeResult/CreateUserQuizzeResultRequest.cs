using TechTrioCourses.Shared.Enums;

namespace UserAPI.DTOs.Request.UserQuizzeResult
{
    public class CreateUserQuizzeResultRequest
    {
        public Guid UserQuizId { get; set; }
        public Guid UserId { get; set; }
        public Guid CourseId { get; set; }
        public Guid QuizId { get; set; }
   
       
    }
}
