using TechTrioCourses.Shared.Enums;

namespace UserAPI.DTOs.Request.UserQuiz
{
    public class UpdateUserQuizRequest
    {
   public UserQuizStatusEnum? Status { get; set; }
  public int? AttemptCount { get; set; }
     public double? BestScore { get; set; }
        public DateTime? FirstAttemptAt { get; set; }
      public DateTime? LastAttemptAt { get; set; }
  public DateTime? PassedAt { get; set; }
 }
}
