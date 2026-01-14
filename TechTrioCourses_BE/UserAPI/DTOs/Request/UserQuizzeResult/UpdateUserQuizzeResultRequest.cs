using TechTrioCourses.Shared.Enums;

namespace UserAPI.DTOs.Request.UserQuizzeResult
{
    public class UpdateUserQuizzeResultRequest
    {
        public double? Score { get; set; }
        public UserQuizResultStatusEnum? Status { get; set; }
        public DateTime? CompletedAt { get; set; }
        public int? DurationSeconds { get; set; }
        public string? Metadata { get; set; }
    }
}
