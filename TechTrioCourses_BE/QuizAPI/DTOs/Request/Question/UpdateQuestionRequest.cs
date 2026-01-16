using TechTrioCourses.Shared.Enums;

namespace QuizAPI.DTOs.Request.Question
{
    public class UpdateQuestionRequest
    {
        public string? QuestionText { get; set; }
        public QuestionTypeEnum? QuestionType { get; set; }
        public PublishStatusEnum? Status { get; set; }
        public double? Points { get; set; }
    }
}
