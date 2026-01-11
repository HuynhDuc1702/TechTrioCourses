using System.ComponentModel;

namespace UserAPI.Enums
{
    public enum UserCourseStatus
    {
        [Description("Dropped")]
        Dropped = 1,
        [Description("In_progress")]
        In_progress = 2,
        [Description("Completed")]
        Completed = 3,
    }
}
