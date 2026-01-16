using System;
using System.Collections.Generic;

namespace UserAPI.Models;

public partial class UserSelectedChoice
{
    public Guid Id { get; set; }

    public Guid ResultId { get; set; }

    public Guid QuestionId { get; set; }

    public Guid ChoiceId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}
