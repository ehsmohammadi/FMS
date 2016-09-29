using System.ComponentModel;
namespace MITD.Fuel.Presentation.Contracts.Enums
{
    public enum DivisionMethodEnum
    {
        [Description(" ")]
        None = 0,
        [Description("براساس مقدار")]
        WithAmount = 1,
        [Description("براساس قیمت")]
        WithPrice = 2,
        [Description("مستقیم")]
        Direct = 3
    }
}