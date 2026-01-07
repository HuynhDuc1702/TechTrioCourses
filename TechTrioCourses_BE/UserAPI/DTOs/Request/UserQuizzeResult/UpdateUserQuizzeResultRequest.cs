using UserAPI.Enums;

namespace UserAPI.DTOs.Request.UserQuizzeResult
{
    public class UpdateUserQuizzeResultRequest
    {
        public double? Score { get; set; }
        public UserQuizzeResultStatusEnum? Status { get; set; }
        public DateTime? CompletedAt { get; set; }
        public int? DurationSeconds { get; set; }
        public string? Metadata { get; set; }
    }
}
