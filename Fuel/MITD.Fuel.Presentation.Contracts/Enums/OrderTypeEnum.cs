using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MITD.Fuel.Presentation.Contracts.Enums
{
    public enum OrderTypeEnum
    {
        [Description("همه")]
        None = 0,
        
        [Description("خرید عمومی")]
        Purchase = 1,
        
        [Description("انتقال")]
        Transfer = 2,
        
        [Description("خرید + انتفال")]
        PurchaseWithTransferOperations = 3,
        
        [Description("انتقال داخلی")]
        InternalTransfer = 4,

        [Description("خرید")]
        PurchaseForVessel = 5,

        [Description("خرید و تحویل مستقیم")]
        SupplyForDeliveredVessel = 6,
    }
}