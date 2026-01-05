using UserAPI.Enums;

namespace UserAPI.DTOs.Request.QuizzeResult
{
    public class CreateQuizzeResultRequest
    {
        public Guid UserId { get; set; }
        public Guid CourseId { get; set; }
        public Guid QuizId { get; set; }
        public int AttemptNumber { get; set; }
        public QuizzeResultStatusEnum Status { get; set; }
        public string? Metadata { get; set; }
    }
}
