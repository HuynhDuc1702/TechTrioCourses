namespace QuizAPI.DTOs.Response.UserSelectedChoice
{
    public class UserSelectedChoiceResponse
    {
      public Guid Id { get; set; }
   public Guid ResultId { get; set; }
        public Guid QuestionId { get; set; }
   public Guid ChoiceId { get; set; }
        public DateTime CreatedAt { get; set; }
   public DateTime UpdatedAt { get; set; }
 }
}
