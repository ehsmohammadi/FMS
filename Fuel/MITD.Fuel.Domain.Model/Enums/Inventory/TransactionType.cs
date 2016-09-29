using System.ComponentModel;

namespace MITD.Fuel.Domain.Model.Enums
{
    /// <summary>
    /// Enumerates the valid base Inventory Operations AKA Inventory Transactions.
    /// </summary>
    public enum TransactionType
    {
        [Description("رسید")]
        Receipt = 1,
        
        [Description("حواله")]
        Issue = 2,
        
        [Description("فاکتور")]
        SaleFactor = 3
    }
}