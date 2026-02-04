using TechTrioCourses.Shared.Enums;

namespace UserAPI.DTOs.Projections.AttemptUserQuizzeResultDetailProjections
{
    public class UserQuizzeResultDetailResponseProjection
    {
        public Guid ResultId { get; set; }
        public Guid QuizId { get; set; }
        public Guid UserQuizId { get; set; }
        public double? Score { get; set; }
        public int AttemptNumber { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public UserQuizResultStatusEnum Status { get; set; }
        public int? DurationSeconds { get; set; }
        public string? Metadata { get; set; }

        public List<UserQuizzeResultQuestionAnswerProjection> Answers { get; set; } = [];
    }
}
