using QuizAPI.Enums;

namespace QuizAPI.DTOs.Response.Question
{
    public class QuestionResponse
    {
        public Guid Id { get; set; }
   public Guid QuizId { get; set; }
   public string QuestionText { get; set; } = null!;
        public QuestionTypeEnum QuestionType { get; set; }
   public QuestionStatusEnum Status { get; set; }
   public double Points { get; set; }
        public DateTime CreatedAt { get; set; }
   public DateTime UpdatedAt { get; set; }
        public string? CorrectAnswer { get; set; }
    }
}
