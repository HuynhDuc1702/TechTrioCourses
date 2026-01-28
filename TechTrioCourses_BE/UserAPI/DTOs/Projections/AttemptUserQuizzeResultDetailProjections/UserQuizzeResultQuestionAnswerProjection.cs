namespace UserAPI.DTOs.Projections.AttemptUserQuizzeResultDetailProjections
{
    public class UserQuizzeResultQuestionAnswerProjection
    {
        public Guid QuestionId { get; set; }

        public string? TextAnswer { get; set; }

        public List<Guid>? SelectedChoiceIds { get; set; }
    }
}
