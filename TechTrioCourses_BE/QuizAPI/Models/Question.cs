using QuizAPI.Enums;
using System;
using System.Collections.Generic;

namespace QuizAPI.Models;

public partial class Question
{
    public Guid Id { get; set; }

    public Guid QuizId { get; set; }

    public string QuestionText { get; set; } = null!;

    public QuestionTypeEnum QuestionType { get; set; }

    public QuestionStatusEnum Status { get; set; }

    public double Points { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}
