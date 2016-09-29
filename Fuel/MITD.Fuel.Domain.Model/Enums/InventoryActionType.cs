using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MITD.Fuel.Domain.Model.Enums
{
    /// <summary>
    /// Enumerates the Actions performed in Inventory from the Fuel Point of View.
    /// </summary>
    public enum InventoryActionType : int
    {
        Issue = 1,
        Receipt = 2,
        Pricing = 3,
        AdjustmentIssue = 4,
        AdjustmentReceipt = 5,
    }
}
