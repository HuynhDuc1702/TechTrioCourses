using System.ComponentModel;
namespace AccountAPI.Enums
{
    public enum AccountStatusEnum
    {
        [Description("Disabled")]
        Disable = 0,
        [Description("Active")]
        Active = 1,
        [Description("Banned")]
        Banned = 2,
    }
}
