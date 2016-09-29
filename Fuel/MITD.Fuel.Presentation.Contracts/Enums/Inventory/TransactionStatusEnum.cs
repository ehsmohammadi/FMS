using System.ComponentModel;

namespace MITD.Fuel.Presentation.Contracts.Enums
{
    public enum TransactionStatusEnum
    {
        [Description("ثبت اولیه")]
        JustRegistered = 1,
        
        [Description("در حال قیمت گذاری")]
        PartialPriced = 2,

        [Description("قیمت گذاری تمام")]
        FullPriced = 3,
        
        [Description("ثبت سند")]
        Vouchered = 4
    }
}