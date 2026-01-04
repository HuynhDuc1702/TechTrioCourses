namespace QuizAPI.DTOs.Request.QuestionAnswer
{
    public class CreateQuestionAnswerRequest
    {
        public Guid QuestionId { get; set; }
        public string AnswerText { get; set; } = null!;
    }
}
