using TechTrioCourses.Shared.Enums;
namespace UserAPI.DTOs.Response.UserQuiz
{
    public class UserQuizResponse
    {
        public Guid Id { get; set; }
   public Guid UserId { get; set; }
        public Guid QuizId { get; set; }
        public Guid CourseId { get; set; }
   public UserQuizStatusEnum Status { get; set; }
   public int AttemptCount { get; set; }
public double? BestScore { get; set; }
        public DateTime? FirstAttemptAt { get; set; }
  public DateTime? LastAttemptAt { get; set; }
 public DateTime? PassedAt { get; set; }
     public DateTime UpdatedAt { get; set; }
    }
}
