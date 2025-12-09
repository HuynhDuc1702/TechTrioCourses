using QuizAPI.Enums;

namespace QuizAPI.DTOs.Request.Quiz
{
    public class UpdateQuizRequest
    {
     public string? Name { get; set; }
  public string? Description { get; set; }
   public double? TotalMarks { get; set; }
 public QuizzStatusEnum? Status { get; set; }
        public double? DurationMinutes { get; set; }
    }
}
