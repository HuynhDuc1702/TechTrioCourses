using System.ComponentModel;

namespace LessonAPI.Enums
{
    public enum LessonStatusEnum
    {
   
        [Description("Hidden")]
     Hidden = 1,
        [Description("Published")]
        Published = 2,
   [Description("Archived")]
        Archived = 3,
       
    }
}