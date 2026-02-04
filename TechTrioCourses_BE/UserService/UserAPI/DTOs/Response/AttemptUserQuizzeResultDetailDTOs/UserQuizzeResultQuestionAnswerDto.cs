namespace UserAPI.DTOs.Response.AttemptUserQuizzeResultDetailDTOs
{
    public class UserQuizzeResultQuestionAnswerDtos
    {
        public Guid QuestionId { get; set; }

        public string? TextAnswer { get; set; }

        public List<Guid>? SelectedChoiceIds { get; set; }
    }
}
