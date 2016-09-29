using System.ComponentModel;

namespace MITD.Fuel.Presentation.Contracts.Enums
{
    public enum TransactionTypeEnum
    {
        [Description("رسید")]
        Receipt = 1,
        
        [Description("حواله")]
        Issue = 2,
        
        [Description("فاکتور")]
        SaleFactor = 3
    }
}