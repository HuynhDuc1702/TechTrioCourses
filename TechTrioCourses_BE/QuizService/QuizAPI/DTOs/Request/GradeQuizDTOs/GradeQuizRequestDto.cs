using TechTrioCourses.Shared.Enums;

namespace QuizAPI.DTOs.Request.GradeQuizDTOs
{
    public class GradeQuizRequestDto
    {
        public Guid QuizId { get; set; }
        public List<UserQuestionAnswersDtos> Answers { get; set; } = [];

    }
    public class UserQuestionAnswersDtos
    {
        public Guid QuestionId { get; set; }
        public QuestionTypeEnum QuestionType { get; set; }
        public List<Guid>? SelectedChoices { get; set; }
        public string? TextAnswer { get; set; }
    }
    public class GradingResultDto
    {
        public Guid QuizId { get; set; }
        public double TotalPointsEarned { get; set; }
        public double TotalMarks { get; set; }
        public double PercentageScore { get; set; }

        public List<GradedQuestionDto> GradedQuestions { get; set; } = [];
        public bool IsPassed { get; set; }

    }
    public class GradedQuestionDto
    {
        public Guid QuestionId { get; set; }
        public string? FeedBack { get; set; }
        public double PointsEarned { get; set; }
        public double MaxPoints { get; set; }
        public bool IsCorrect { get; set; }
    }
}
