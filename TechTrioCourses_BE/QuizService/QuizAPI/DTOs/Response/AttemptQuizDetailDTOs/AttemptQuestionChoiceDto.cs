namespace QuizAPI.DTOs.Response.AttemptQuizDetailDTOs
{
    public class AttemptQuestionChoiceDto
    {
        public Guid Id { get; set; }
        public string ChoiceText { get; set; } = null!;
    }
}
