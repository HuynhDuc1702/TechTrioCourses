namespace QuizAPI.DTOs.Request.QuestionAnswer
{
    public class CreateQuestionAnswerRequest
    {
        public Guid ResultId { get; set; }
 public Guid QuestionId { get; set; }
     public string AnswerText { get; set; } = null!;
    public string? CorrectAnswer { get; set; }
    }
}
