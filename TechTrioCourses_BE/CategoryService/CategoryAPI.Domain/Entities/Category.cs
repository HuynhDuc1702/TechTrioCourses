
using TechTrioCourses.Shared.Abstractions;
namespace CategoryAPI.Domain.Entities;

public partial class Category : BaseEntity
{

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

}
