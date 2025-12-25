using UserAPI.Enums;

namespace UserAPI.DTOs.Request
{
    public class CreateUserQuizRequest
    {
        public Guid UserId { get; set; }
  public Guid QuizId { get; set; }

        public int AttemptCount { get; set; }
        public double? BestScore { get; set; }
    }
}
