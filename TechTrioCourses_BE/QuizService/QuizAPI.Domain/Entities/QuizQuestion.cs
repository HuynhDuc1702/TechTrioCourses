

namespace QuizAPI.Domain.Entities;

public partial class QuizQuestion
{
    public Guid QuizId { get; set; }

    public Guid QuestionId { get; set; }

    public int? QuestionOrder { get; set; }

    public double? OverridePoints { get; set; }

    public virtual Question Question { get; set; } = null!;

    public virtual Quiz Quiz { get; set; } = null!;
}
