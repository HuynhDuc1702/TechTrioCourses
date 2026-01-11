using TechTrioCourses.Shared.Enums;

namespace UserAPI.DTOs.Response.UserQuizzeResult
{
    public class UserQuizzeResultResponse
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid CourseId { get; set; }
        public Guid QuizId { get; set; }
        public double? Score { get; set; }
        public UserQuizResultStatusEnum Status { get; set; }
        public int AttemptNumber { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public int? DurationSeconds { get; set; }
        public string? Metadata { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
