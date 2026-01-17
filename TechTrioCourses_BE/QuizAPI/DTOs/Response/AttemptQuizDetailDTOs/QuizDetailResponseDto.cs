namespace QuizAPI.DTOs.Response.AttemptQuizDetailDTOs
{
    public class QuizDetailResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public double DurationMinutes { get; set; }

        public List<QuizQuestionDto> Questions { get; set; } = [];
    }
}
