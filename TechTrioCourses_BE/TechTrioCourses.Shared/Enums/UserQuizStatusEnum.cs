using System.ComponentModel;

namespace TechTrioCourses.Shared.Enums
{
    public enum UserQuizStatusEnum
    {
        [Description("Not_Started")]
        Not_Started = 1,
        [Description("In_progress")]
        In_progress = 2,
        [Description("Passed")]
  Passed = 3,
        [Description("Failed")]
        Failed = 4
    }
}
