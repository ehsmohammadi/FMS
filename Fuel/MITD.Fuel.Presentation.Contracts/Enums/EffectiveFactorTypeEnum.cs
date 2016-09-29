using System.ComponentModel;

namespace MITD.Fuel.Presentation.Contracts.Enums
{
    public enum EffectiveFactorTypeEnum
    {
        [Description("کاهنده")]
        Decrease = -1,
        [Description("افزاینده")]
        InCrease = 1
    }
}