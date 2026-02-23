using TechTrioCourses.Shared.Enums;
using System;
using System.Collections.Generic;
using TechTrioCourses.Shared.Abstractions;

namespace QuizAPI.Domain.Entities;

public partial class Quiz :BaseEntity
{


    public Guid CourseId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public double TotalMarks { get; set; }

    public PublishStatusEnum Status { get; set; }

    public double DurationMinutes { get; set; }


    public virtual ICollection<QuizQuestion> QuizQuestions { get; set; } = new List<QuizQuestion>();
}
