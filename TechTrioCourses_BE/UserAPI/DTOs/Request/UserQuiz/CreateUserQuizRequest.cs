namespace UserAPI.DTOs.Request.UserQuiz
{
    public class CreateUserQuizRequest
    {
        public Guid UserId { get; set; }
        public Guid QuizId { get; set; }
        public Guid CourseId { get; set; }


    }
}
