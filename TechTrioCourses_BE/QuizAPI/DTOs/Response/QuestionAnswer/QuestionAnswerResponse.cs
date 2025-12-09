namespace QuizAPI.DTOs.Response.QuestionAnswer
{
    public class QuestionAnswerResponse
    {
        public Guid Id { get; set; }
   public Guid ResultId { get; set; }
   public Guid QuestionId { get; set; }
        public string AnswerText { get; set; } = null!;
      public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
     public string? CorrectAnswer { get; set; }
    }
}
