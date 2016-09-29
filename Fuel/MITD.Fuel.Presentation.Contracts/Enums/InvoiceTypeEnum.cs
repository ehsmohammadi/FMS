using System.ComponentModel;
namespace MITD.Fuel.Presentation.Contracts.DTOs
{
    public enum InvoiceTypeEnum
    {
        [Description("خرید")]
        Purchase = 0,
        [Description("عملیات انتقال (Barging)")]
        PurchaseOperations = 1,
        [Description("پیوست")]
        Attach = 2,
        [Description("خرید و تحویل مستقیم")]
        SupplyForDeliveredVessel = 3,
    }
}