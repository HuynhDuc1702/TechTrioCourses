using TechTrioCourses.Shared.Enums;

namespace QuizAPI.DTOs.Response.AttemptQuizDetailDTOs
{
    public class AttemptQuizQuestionDto
    {
        public Guid QuestionId { get; set; }
        public string QuestionText { get; set; } = null!;
        public QuestionTypeEnum QuestionType { get; set; }
        public double Points { get; set; }
        public int? Order { get; set; }

        public List<AttemptQuestionChoiceDto>? Choices { get; set; }
    }
}
