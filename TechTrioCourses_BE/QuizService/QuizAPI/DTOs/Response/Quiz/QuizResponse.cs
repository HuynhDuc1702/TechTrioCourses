using QuizAPI.DTOs.Projections.AttemptQuizDetailProjections;
using QuizAPI.DTOs.Response.QuizQuestion;
using TechTrioCourses.Shared.Enums;

namespace QuizAPI.DTOs.Response.Quiz
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
        public List<QuizQuestionResponse> QuizQuestions { get; set; } = [];
    }
}
