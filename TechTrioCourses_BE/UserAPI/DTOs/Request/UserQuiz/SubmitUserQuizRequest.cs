using TechTrioCourses.Shared.Enums;
namespace UserAPI.DTOs.Request.UserQuiz
{
    public class ApplyQuizGradingResultRequest
    {
         public double? SubmitScore { get; set; }
     
        public bool IsPassed { get; set; }
        
 }
}
