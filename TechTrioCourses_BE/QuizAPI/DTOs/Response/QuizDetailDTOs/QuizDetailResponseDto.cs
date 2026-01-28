using QuizAPI.DTOs.Response.AttemptQuizDetailDTOs;
using TechTrioCourses.Shared.Enums;

namespace QuizAPI.DTOs.Response.FullQuizDetailDTOs
{
    public class QuizDetailResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public double TotalMarks { get; set; }
        public double DurationMinutes { get; set; }

        public List<QuizQuestionDetailDto> Questions { get; set; } = [];
    }
    public class QuizQuestionDetailDto
    {
        public Guid QuestionId { get; set; }
        public string QuestionText { get; set; } = null!;
        public QuestionTypeEnum QuestionType { get; set; }
        public double Points { get; set; }
        public int? Order { get; set; }

        public List<QuizQuestionChoicesDetailDto>? Choices { get; set; }
        public List<QuizQuestionAnswersDetailDto>? Answers { get; set; }
    }
    public class QuizQuestionChoicesDetailDto
    {
        public Guid Id { get; set; }
        public string ChoiceText { get; set; } = null!;
        public bool IsCorrect { get; set; }
    }
    public class QuizQuestionAnswersDetailDto
    {
        public Guid Id { get; set; }
        public string AnswerText { get; set; } = null!;
    }
}
