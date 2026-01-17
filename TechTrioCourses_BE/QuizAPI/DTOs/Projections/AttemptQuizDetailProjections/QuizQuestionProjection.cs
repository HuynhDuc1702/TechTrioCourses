using TechTrioCourses.Shared.Enums;

namespace QuizAPI.DTOs.Projections.AttemptQuizDetailProjections
{
    public class QuizQuestionProjection
    {
        public Guid QuestionId { get; set; }
        public string QuestionText { get; set; } = null!;
        public QuestionTypeEnum QuestionType { get; set; }
        public double Points { get; set; }
        public int? Order { get; set; }

        public List<QuestionChoiceProjection>? Choices { get; set; }
    }
}
