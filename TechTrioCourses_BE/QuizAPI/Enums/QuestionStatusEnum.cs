using System.ComponentModel;

namespace QuizAPI.Enums
{
    public enum QuestionStatusEnum
    {

        [Description("Hidden")]
        Hidden = 1,
        [Description("Published")]
        Published = 2,
        [Description("Archived")]
        Archived = 3,

    }
}