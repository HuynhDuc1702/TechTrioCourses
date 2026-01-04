using System;
using System.Collections.Generic;

namespace QuizAPI.Models;

public partial class QuestionChoice
{
    public Guid Id { get; set; }

    public Guid QuestionId { get; set; }

    public string ChoiceText { get; set; } = null!;

    public bool IsCorrect { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual Question Question { get; set; } = null!;
}
