using UserAPI.Enums;

namespace UserAPI.DTOs.Request.QuizzeResult
{
    public class UpdateQuizzeResultRequest
    {
        public double? Score { get; set; }
        public QuizzeResultStatusEnum? Status { get; set; }
        public DateTime? CompletedAt { get; set; }
        public int? DurationSeconds { get; set; }
        public string? Metadata { get; set; }
    }
}
