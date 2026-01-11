using System;
using System.Collections.Generic;
using TechTrioCourses.Shared.Enums;

namespace UserAPI.Models;

public partial class UserQuiz
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public Guid QuizId { get; set; }
    public Guid? CourseId { get; set; }

    public UserQuizStatusEnum Status { get; set; }

    public double? BestScore { get; set; }

    public int AttemptCount { get; set; }

    public DateTime? FirstAttemptAt { get; set; }

    public DateTime? PassedAt { get; set; }

    public DateTime? LastAttemptAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}
