namespace QuizAPI.DTOs.Projections.AttemptQuizDetailProjections
{
    public class AttemptQuizDetailResponseProjection
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public double DurationMinutes { get; set; }

        public List<AttemptQuizQuestionProjection> Questions { get; set; } = [];
    }
}
