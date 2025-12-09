using System.ComponentModel;

namespace LessonAPI.Enums
{
    public enum LessonStatusEnum
    {
       
        [Description("Hidden")]
        Hidden = 0,
        [Description("Published")]
        Published = 1,
        [Description("Archived")]
        Archived = 2,
       
    }
}