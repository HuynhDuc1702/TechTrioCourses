

using TechTrioCourses.Shared.Abstractions;

namespace UserAPI.Domain.Entities;

public partial class UserInputAnswer : BaseEntity
{


    public Guid ResultId { get; set; }

    public Guid QuestionId { get; set; }

    public string AnswerText { get; set; } = null!;

 
}
