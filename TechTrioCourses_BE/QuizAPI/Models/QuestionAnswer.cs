using System;
using System.Collections.Generic;

namespace QuizAPI.Models;

public partial class QuestionAnswer
{
    public Guid Id { get; set; }

    public Guid QuestionId { get; set; }

    public string AnswerText { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual Question Question { get; set; } = null!;
}
