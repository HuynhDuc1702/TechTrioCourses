namespace QuizAPI.DTOs.Projections.AttemptQuizDetailProjections
{
    public class AttemptQuestionChoiceProjection
    {
        public Guid Id { get; set; }
        public string ChoiceText { get; set; } = null!;
    }
}
