namespace QuizAPI.DTOs.Response.AttemptQuizDetailDTOs
{
    public class QuestionChoiceDto
    {
        public Guid Id { get; set; }
        public string ChoiceText { get; set; } = null!;
    }
}
