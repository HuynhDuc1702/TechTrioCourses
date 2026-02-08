namespace QuizAPI.Application.DTOs.Request.QuestionChoice
{
    public class UpdateQuestionChoiceRequest
    {
        public string? ChoiceText { get; set; }
        public bool? IsCorrect { get; set; }
    }
}
