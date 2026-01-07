using System.ComponentModel;

namespace CourseAPI.Enums
{
    public enum QuizzStatusEnum
    {
 
        [Description("Hidden")]
        Hidden = 1,
        [Description("Published")]
        Published = 2,
        [Description("Archived")]
        Archived = 3,
       
    }
}