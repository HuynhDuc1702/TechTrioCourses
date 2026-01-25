using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTrioCourses.Shared.Enums;

namespace TechTrioCourses.Shared.Contracts
{
    public record QuizSubmittedEvent
    {
        public Guid ResultId { get; set; }
        public Guid QuizId { get; set; }
        public Guid UserQuizId { get; set; }
        public List<QuestionAnswerEventDto> Answers { get; set; } = [];
        public DateTime SubmittedAt { get; set; }
    }
    public record QuestionAnswerEventDto
    {
        public Guid QuestionId { get; set; }
        public QuestionTypeEnum QuestionType { get; set; }
        public List<Guid>? SelectedChoices { get; set; }
        public string? TextAnswer { get; set; }
    }
}
