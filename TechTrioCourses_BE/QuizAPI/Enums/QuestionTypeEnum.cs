using System.ComponentModel;

namespace QuizAPI.Enums
{
    public enum QuestionTypeEnum
    {

        [Description("Multiple_Choice")]
        Multiple_Choice = 0,
        [Description("True_False")]
        True_False = 1,
        [Description("Short_Answer")]
        Short_Answer = 2,

    }
}