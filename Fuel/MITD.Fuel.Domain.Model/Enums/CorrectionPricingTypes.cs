#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace MITD.Fuel.Domain.Model.Enums
{
    public enum CorrectionPricingTypes
    {
        NotDefined = 0,
        Default = 1,
        LastPurchasePrice = 2,
        LastIssuedConsumption = 3,
        ManualPricing = 4
    }
}