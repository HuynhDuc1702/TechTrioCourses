namespace QuizAPI.DTOs.Response.QuizQuestion
{
    public class QuizQuestionResponse
    {
     
        public Guid QuizId { get; set; }
        public Guid QuestionId { get; set; }
        public int? QuestionOrder { get; set; }
        public double? OverridePoints { get; set; }
    }
}
