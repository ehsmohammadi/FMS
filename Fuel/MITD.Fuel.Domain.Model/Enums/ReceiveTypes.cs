#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace MITD.Fuel.Domain.Model.Enums
{
    public enum ReceiveTypes
    {
        //NotDefined = 0,
        Trust = 1,
        InternalTransfer = 2,
        TransferPurchase = 3,
        Purchase = 4,
    }
}