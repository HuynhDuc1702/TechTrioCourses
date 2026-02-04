namespace QuizAPI.DTOs.Response.QuestionChoice
{
    public class QuestionChoiceResponse
    {
        public Guid Id { get; set; }
        public Guid QuestionId { get; set; }
        public string ChoiceText { get; set; } = null!;
        public bool IsCorrect { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
