using System;
using System.Collections.Generic;
using TechTrioCourses.Shared.Abstractions;

namespace QuizAPI.Domain.Entities;

public partial class QuestionChoice : BaseEntity
{
  

    public Guid QuestionId { get; set; }

    public string ChoiceText { get; set; } = null!;

    public bool IsCorrect { get; set; }

 

    public virtual Question Question { get; set; } = null!;
}
