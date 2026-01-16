using System.ComponentModel;

namespace TechTrioCourses.Shared.Enums
{
    public enum UserRoleEnum
    {
        [Description("Admin")]
        Admin = 1,
        [Description("Student")]
        Student = 2,
        [Description("Instructor")]
        Instructor = 3
    }
}
