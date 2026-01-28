namespace QuizAPI.DTOs.Response.AttemptQuizDetailDTOs
{
    public class AttemptQuizDetailResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public double DurationMinutes { get; set; }

        public List<AttemptQuizQuestionDto> Questions { get; set; } = [];
    }
}
