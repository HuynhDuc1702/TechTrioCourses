using System.ComponentModel;

namespace QuizAPI.Enums
{
    public enum QuizzStatusEnum
    {
       
        [Description("Hidden")]
        Hidden = 0,
        [Description("Published")]
        Published = 1,
        [Description("Archived")]
        Archived = 2,
       
    }
}