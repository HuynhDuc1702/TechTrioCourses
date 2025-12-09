using System.ComponentModel;

namespace QuizAPI.Enums
{
    public enum QuizzResultStatusEnum
    {

        [Description("Pending")]
        Pending = 0,
        [Description("In_Progress")]
        In_Progress = 1,
        [Description("Completed")]
        Completed = 2,

    }
}