using System.ComponentModel;

namespace TechTrioCourses.Shared.Enums
{
    /// <summary>
    /// Represents the publish status for content entities (Course, Lesson, Quiz, Question)
    /// </summary>
    public enum PublishStatusEnum
    {
        [Description("Hidden")]
        Hidden = 1,
        [Description("Published")]
        Published = 2,
        [Description("Archived")]
        Archived = 3
    }
}
