namespace UserAPI.DTOs.Projections.AttemptUserQuizzeResultDetailProjections
{
    public class UserQuizzeResultDetailResponseProjection
    {
        public Guid ResultId { get; set; }
        public Guid QuizId { get; set; }
        public Guid UserId { get; set; }
        public int AttemptNumber { get; set; }
        public double? Score { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }

        public List<UserQuizzeResultQuestionAnswerProjection> Answers { get; set; } = [];
    }
}
