namespace QuizAPI.DTOs.Projections.AttemptQuizDetailProjections
{
    public class QuestionChoiceProjection
    {
        public Guid Id { get; set; }
        public string ChoiceText { get; set; } = null!;
    }
}
