using QuizAPI.Enums;

namespace QuizAPI.DTOs.Request.Question
{
    public class CreateQuestionRequest
    {
        public Guid UserId { get; set; }
        public Guid CourseId { get; set; }
        public string QuestionText { get; set; } = null!;
        public QuestionTypeEnum QuestionType { get; set; }
        public double Points { get; set; }
        public QuestionStatusEnum Status { get; set; }
    }
}
