using QuizAPI.Enums;
using System;
using System.Collections.Generic;

namespace QuizAPI.Models;

public partial class Quiz
{
    public Guid Id { get; set; }

    public Guid CourseId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public double TotalMarks { get; set; }

    public QuizzStatusEnum Status { get; set; }

    public double DurationMinutes { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}
