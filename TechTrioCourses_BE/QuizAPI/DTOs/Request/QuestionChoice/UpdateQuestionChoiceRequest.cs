namespace QuizAPI.DTOs.Request.QuestionChoice
{
    public class UpdateQuestionChoiceRequest
    {
   public string? OptionText { get; set; }
        public bool? IsCorrect { get; set; }
    }
}
