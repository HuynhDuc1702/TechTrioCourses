namespace QuizAPI.DTOs.Request.QuestionChoice
{
    public class CreateQuestionChoiceRequest
    {
        public Guid QuestionId { get; set; }
        public string ChoiceText { get; set; } = null!;
        public bool? IsCorrect { get; set; }
    }
}
