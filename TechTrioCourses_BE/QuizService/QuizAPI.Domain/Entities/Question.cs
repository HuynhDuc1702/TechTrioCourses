using TechTrioCourses.Shared.Enums;
using System;
using System.Collections.Generic;
using TechTrioCourses.Shared.Abstractions;

namespace QuizAPI.Domain.Entities;

public partial class Question : BaseEntity
{


    public Guid UserId { get; set; }

    public Guid CourseId { get; set; }

    public string QuestionText { get; set; } = null!;

    public QuestionTypeEnum QuestionType { get; set; }

    public PublishStatusEnum Status { get; set; }

    public double Points { get; set; }



    public virtual ICollection<QuestionAnswer> QuestionAnswers { get; set; } = new List<QuestionAnswer>();

    public virtual ICollection<QuestionChoice> QuestionChoices { get; set; } = new List<QuestionChoice>();

    public virtual ICollection<QuizQuestion> QuizQuestions { get; set; } = new List<QuizQuestion>();
}
