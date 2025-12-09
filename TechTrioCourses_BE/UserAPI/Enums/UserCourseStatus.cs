using System.ComponentModel;

namespace UserAPI.Enums
{
    public enum UserCourseStatus
    {
        [Description("Dropped")]
        Dropped = 0,
        [Description("In_progress")]
        In_progress = 1,
        [Description("Completed")]
        Completed = 2,
    }
}
