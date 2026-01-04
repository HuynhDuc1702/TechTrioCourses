using QuizAPI.Enums;

namespace QuizAPI.DTOs.Response.Question
{
    public class QuestionResponse
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid CourseId { get; set; }
        public string QuestionText { get; set; } = null!;
        public QuestionTypeEnum QuestionType { get; set; }
        public QuestionStatusEnum Status { get; set; }
        public double Points { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
