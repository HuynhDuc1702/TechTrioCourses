using System.ComponentModel;

namespace LessonAPI.Enums
{
    public enum LessonMediaTypeEnum
    {

        [Description("Video")]
        Video = 1,
        [Description("Audio")]
        Audio = 2,
        [Description("Document")]
        Document = 3,
        [Description("Image")]
        Image = 4,

    }
}