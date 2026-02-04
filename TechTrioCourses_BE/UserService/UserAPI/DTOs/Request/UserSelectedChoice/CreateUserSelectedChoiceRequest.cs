namespace UserAPI.DTOs.Request.UserSelectedChoice
{
    public class CreateUserSelectedChoiceRequest
    {
        public Guid ResultId { get; set; }
        public Guid QuestionId { get; set; }
        public Guid ChoiceId { get; set; }
    }
}
