using System;
using System.Collections.Generic;

namespace UserAPI.Models;

public partial class UserInputAnswer
{
    public Guid Id { get; set; }

    public Guid ResultId { get; set; }

    public Guid QuestionId { get; set; }

    public string AnswerText { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}
