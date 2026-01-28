using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechTrioCourses.Shared.Contracts
{
    public record QuizGradedEvent
    {
        public Guid QuizId { get; set; }
        public Guid ResultId { get; set; }
        public Guid UserQuizId { get; set; }
        public double TotalPointsEarned { get; set; }
        public double TotalMarks { get; set; }
        public double PercentageScore { get; set; }
        public bool IsPassed { get; set; }
        public List<GradedQuestionEvent> GradedQuestions { get; set; } = [];
        public DateTime GradedAt { get; set; }
    }
    public record GradedQuestionEvent
    {
        public Guid QuestionId { get; set; }
        public bool IsCorrect { get; set; }
        public double PointsEarned { get; set; }
        public double MaxPoints { get; set; }
    }
}
