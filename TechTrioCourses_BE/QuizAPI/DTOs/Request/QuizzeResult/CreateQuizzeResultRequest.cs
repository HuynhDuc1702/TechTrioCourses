namespace QuizAPI.DTOs.Request.QuizzeResult
{
    public class CreateQuizzeResultRequest
    {
   public Guid UserId { get; set; }
     public Guid CourseId { get; set; }
        public Guid QuizId { get; set; }
        public int AttemptNumber { get; set; }
    }
}
