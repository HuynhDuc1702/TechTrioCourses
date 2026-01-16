using TechTrioCourses.Shared.Enums;
namespace UserAPI.DTOs.Request.UserQuiz
{
    public class SubmitUserQuizRequest
    {
         public double? SubmitScore { get; set; }
     
        public bool? isPassed { get; set; }
 }
}
