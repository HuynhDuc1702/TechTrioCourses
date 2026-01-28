using System.ComponentModel;

namespace TechTrioCourses.Shared.Enums
{
    public enum UserQuizResultStatusEnum
    {
        [Description("In_progress")]
        In_progress = 1,
        [Description("Grading")]
        Grading = 2,
        [Description("Passed")]
        Passed=3,
        [Description("Failed")]
        Failed = 4,
    }
}
