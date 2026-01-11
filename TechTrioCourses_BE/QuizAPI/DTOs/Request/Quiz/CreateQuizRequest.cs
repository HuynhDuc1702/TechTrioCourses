using TechTrioCourses.Shared.Enums;

namespace QuizAPI.DTOs.Request.Quiz
{
    public class CreateQuizRequest
    {
        public Guid CourseId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public double TotalMarks { get; set; }
        public double DurationMinutes { get; set; }
        public PublishStatusEnum Status { get; set; }
    }
}
