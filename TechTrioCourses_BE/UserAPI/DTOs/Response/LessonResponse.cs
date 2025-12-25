namespace UserAPI.DTOs.Response
{
    public class LessonResponse
    {
     public Guid Id { get; set; }
     public Guid CourseId { get; set; }
      public string Title { get; set; } = null!;
        public string? Content { get; set; }
        public int Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
