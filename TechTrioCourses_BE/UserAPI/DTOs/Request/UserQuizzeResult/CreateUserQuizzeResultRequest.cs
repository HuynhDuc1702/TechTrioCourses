using UserAPI.Enums;

namespace UserAPI.DTOs.Request.UserQuizzeResult
{
    public class CreateUserQuizzeResultRequest
    {
        public Guid UserId { get; set; }
        public Guid CourseId { get; set; }
        public Guid QuizId { get; set; }
        public int AttemptNumber { get; set; }
        public UserQuizzeResultStatusEnum Status { get; set; }
        public string? Metadata { get; set; }
    }
}
