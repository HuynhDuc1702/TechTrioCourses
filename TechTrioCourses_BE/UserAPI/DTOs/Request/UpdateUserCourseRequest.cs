using UserAPI.Enums;

namespace UserAPI.DTOs.Request
{
    public class UpdateUserCourseRequest
    {
        public UserCourseStatus? Status { get; set; }
      
        public DateTime? CompletedAt { get; set; }
 }
}
