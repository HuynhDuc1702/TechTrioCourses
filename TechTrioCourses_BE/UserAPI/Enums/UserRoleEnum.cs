using System.ComponentModel;

namespace UserAPI.Enums
{
    
        public enum UserRoleEnum
        {
            [Description("Admin")]
            Admin = 0,
            [Description("Student")]
            Student = 1,
            [Description("Instructor")]
            Instructor = 2,
        }
    
}
