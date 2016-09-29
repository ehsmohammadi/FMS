using System.ComponentModel;

namespace MITD.Fuel.Presentation.Contracts.Enums
{
    public enum ReceiveTypeEnum : int
    {
        [Description(" ")]
        NotDefined = 0,

        [Description("امانی")]
        Trust = 1,

        [Description("انتقال داخلی")]
        InternalTransfer = 2,

        [Description("خرید + انتقال")]
        TransferPurchase = 3,

        [Description("خرید")]
        Purchase = 4,
    }
}
