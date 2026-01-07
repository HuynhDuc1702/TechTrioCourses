using CourseAPI.Enums;

namespace CourseAPI.DTOs.Response
{
    public class QuizResponse
 {
        public Guid Id { get; set; }
        public Guid CourseId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
  public double TotalMarks { get; set; }
        public QuizzStatusEnum Status { get; set; }
        public double DurationMinutes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
