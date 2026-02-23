using System;
using System.Collections.Generic;
using TechTrioCourses.Shared.Abstractions;

namespace QuizAPI.Domain.Entities;

public partial class QuestionAnswer : BaseEntity
{


    public Guid QuestionId { get; set; }

    public string AnswerText { get; set; } = null!;

    

    public virtual Question Question { get; set; } = null!;
}
