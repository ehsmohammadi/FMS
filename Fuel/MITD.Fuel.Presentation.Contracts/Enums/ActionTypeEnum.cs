using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MITD.Fuel.Presentation.Contracts.Enums
{
    public enum ActionTypeEnum
    {
        Undefined = 0,
        Created = 1,
        Approved = 2,
        //FinalApproved = 3,  //Deprecated and not used, but the id of other ActionTypes should remain unchanged.
        Rejected = 4,

    }

    public enum ActionEntityTypeEnum
    {
        Order = 101,
        FuelReport = 102,
        Invoice = 103,
        Scrap = 104,
        Offhire = 107,
        CharterIn = 105,
        CharterOut = 106
    }

    public enum DecisionTypeEnum
    {
        Approved,
        Rejected,
        Canceled,
        Close
    }
}
