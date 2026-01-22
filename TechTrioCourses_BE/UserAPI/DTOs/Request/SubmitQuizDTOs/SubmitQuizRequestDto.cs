using TechTrioCourses.Shared.Enums;

namespace UserAPI.DTOs.Request.SubmitQuizDTOs
{
    public class SubmitQuizRequestDto
    {
        public Guid ResultId { get; set; }
        public Guid UserQuizId { get; set; }
        public List<QuestionAnswersDtos> Answers { get; set; } = [];

        public int? DurationSeconds { get; set; }
        public bool IsFinalSubmisson { get; set; }
    }
    public class QuestionAnswersDtos
    {
        public Guid QuestionId { get; set; }
        public QuestionTypeEnum QuestionType { get; set; }
        public List<Guid>? SelectedChoices { get; set; }
        public string? InputAnswer { get; set; }
    }
    public class SubmitQuizResponseDto
    {
        public Guid ResultId { get; set; }
        public Guid UserQuizId { get; set; }
        public string? Message { get; set; }
        public UserQuizResultStatusEnum Status { get; set; }

        public double? Score { get; set; }
        public bool? IsPased { get; set; }

    }
}
