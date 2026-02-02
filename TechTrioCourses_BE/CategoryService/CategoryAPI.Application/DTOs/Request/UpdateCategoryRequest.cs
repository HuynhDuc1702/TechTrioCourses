namespace CategoryAPI.Application.DTOs.Request
{
    public class UpdateCategoryRequest
    {
        public string Name { get; set; } = null!;

        public string? Description { get; set; }
    }
}
