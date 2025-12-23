using System.ComponentModel;

namespace QuizAPI.Enums
{
    public enum QuizzResultStatusEnum
    {

        [Description("Pending")]
        Pending = 1,
        [Description("In_Progress")]
        In_Progress = 2,
        [Description("Completed")]
        Completed = 3,


    }
}