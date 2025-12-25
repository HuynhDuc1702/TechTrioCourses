using System.ComponentModel;

namespace UserAPI.Enums
{
    public enum UserQuizzStatus
    {
        [Description("Not_Started")]
        Not_Started = 1,
        [Description("In_progress")]
        In_progress = 2,
        [Description("Passed")]
        Passed = 3,
        [Description("Failed")]
        Failed = 4,

    }
}

