
using TechTrioCourses.Shared.Enums;

namespace QuizAPI.DTOs.Projections.FullQuizDetailProjections
{
    public class QuizDetailProjection
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        
        public double TotalMarks { get; set; }

        public string? Description { get; set; }
        public double DurationMinutes { get; set; }

        public List<QuizQuestionDetailProjection> Questions { get; set; } = [];
    }
    public class QuizQuestionDetailProjection
    {
        public Guid QuestionId { get; set; }
        public string QuestionText { get; set; } = null!;
        public QuestionTypeEnum QuestionType { get; set; }
        public double Points { get; set; }
        public int? Order { get; set; }

        public List<QuizQuestionChoicesDetailProjection>? Choices { get; set; }
        public List<QuizQuestionAnswersDetailProjection>? Answers { get; set; }
    }
    public class QuizQuestionChoicesDetailProjection
    {
        public Guid Id { get; set; }
        public string ChoiceText { get; set; } = null!;
        public bool IsCorrect { get; set; }
    }
    public class QuizQuestionAnswersDetailProjection
    {
        public Guid Id { get; set; }
        public string AnswerText { get; set; } = null!;
    }
}
