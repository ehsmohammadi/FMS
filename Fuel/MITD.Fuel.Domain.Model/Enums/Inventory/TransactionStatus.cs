using System.ComponentModel;

namespace MITD.Fuel.Domain.Model.Enums
{
    public enum TransactionState
    {
        [Description("ثبت اولیه")]
        JustRegistered = 1,
        
        [Description("قیمت گذاری")]
        PartialPriced = 2,

        [Description("قیمت گذاری تمام")]
        FullPriced = 3,
        
        [Description("ثبت سند")]
        Vouchered = 4
    }
}