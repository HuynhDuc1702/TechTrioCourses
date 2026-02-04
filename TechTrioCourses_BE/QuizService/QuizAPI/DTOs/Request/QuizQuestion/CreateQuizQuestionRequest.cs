namespace QuizAPI.DTOs.Request.QuizQuestion
{
    public class CreateQuizQuestionRequest
    {
        public Guid QuizId { get; set; }
        public Guid QuestionId { get; set; }
   public int? QuestionOrder { get; set; }
        public double? OverridePoints { get; set; }
    }
}
