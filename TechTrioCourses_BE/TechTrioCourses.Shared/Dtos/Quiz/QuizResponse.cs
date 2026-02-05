using TechTrioCourses.Shared.Enums;

namespace TechTrioCourses.Shared.Dtos.Quiz
{
    public class QuizResponse
 {
        public Guid Id { get; set; }
        public Guid CourseId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
  public double TotalMarks { get; set; }
        public PublishStatusEnum Status { get; set; }
        public double DurationMinutes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
