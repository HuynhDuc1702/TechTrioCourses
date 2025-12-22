using System.ComponentModel;
namespace AccountAPI.Enums
{
    public enum AccountStatusEnum
    {
        [Description("Disabled")]
        Disable = 1,
        [Description("Active")]
        Active = 2,
        [Description("Banned")]
        Banned = 3,
    }
}
