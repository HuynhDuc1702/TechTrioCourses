namespace UserAPI.DTOs.Request.UserInputAnswer
{
    public class CreateUserInputAnswerRequest
    {
        public Guid ResultId { get; set; }
        public Guid QuestionId { get; set; }
  public string AnswerText { get; set; } = null!;
}
}
