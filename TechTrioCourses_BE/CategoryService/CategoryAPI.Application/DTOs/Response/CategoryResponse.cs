namespace CategoryAPI.Application.DTOs.Response
{
    public class CategoryResponse
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }
    }
}
