using LessonAPI.Enums;

namespace LessonAPI.DTOs.Request
{
    public class UpdateLessonRequest
    {
      

        public string? Title { get; set; } 

        public string? Content { get; set; }

        public string? MediaUrl { get; set; }

        public LessonMediaTypeEnum? MediaType { get; set; }

        public int? OrderIndex { get; set; }
            
        public LessonStatusEnum? Status { get; set; } 
    }
}
