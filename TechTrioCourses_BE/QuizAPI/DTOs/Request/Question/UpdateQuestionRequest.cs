using QuizAPI.Enums;

namespace QuizAPI.DTOs.Request.Question
{
    public class UpdateQuestionRequest
    {
        public string? QuestionText { get; set; }
        public QuestionTypeEnum? QuestionType { get; set; }
        public QuestionStatusEnum? Status { get; set; }
        public double? Points { get; set; }
    }
}
