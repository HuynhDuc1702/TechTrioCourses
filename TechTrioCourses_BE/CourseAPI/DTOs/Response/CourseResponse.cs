using CourseAPI.Models;
using CourseAPI.Enums;

namespace CourseAPI.DTOs.Response
{
    
        public class CourseResponse
        {
            public Guid Id { get; set; }

            public string Title { get; set; } = null!;
            public string? Description { get; set; }
            public Guid? CategoryId { get; set; }
            public Guid? CreatorId { get; set; }
            public CourseStatusEnum Status { get; set; }

            public DateTime? CreatedAt { get; set; }
            public DateTime? UpdatedAt { get; set; }

            // Optional related mapping
            public string? CategoryName { get; set; }
            public string? CreatorName { get; set; }

            public int TotalLessons { get; set; }
            public int TotalQuizzes { get; set; }
            public double AverageRating { get; set; }
        }

    
}
