namespace UserAPI.DTOs.Response.AttemptUserQuizzeResultDetailDTOs
{
    public class UserQuizzeResultResumeResponseDto
    {
        public Guid ResultId { get; set; }
        public Guid QuizId { get; set; }
        public Guid UserQuizId { get; set; }
        public int AttemptNumber { get; set; }
   
        public DateTime StartedAt { get; set; }
       

        public List<UserQuizzeResultQuestionAnswerDtos> Answers { get; set; } = [];
    }
}
