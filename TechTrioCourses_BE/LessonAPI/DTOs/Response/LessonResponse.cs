using TechTrioCourses.Shared.Enums;
using LessonAPI.Models;


namespace LessonAPI.DTOs.Response
{
    
        public class LessonResponse
        {
        public Guid Id { get; set; }

        public Guid CourseId { get; set; }
        public string? CourseName { get; set; }

        public string Title { get; set; } = null!;

        public string? Content { get; set; }

        public string? MediaUrl { get; set; }

        public LessonMediaTypeEnum? MediaType { get; set; }

        public int? OrderIndex { get; set; }

        public PublishStatusEnum Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }

    
}
