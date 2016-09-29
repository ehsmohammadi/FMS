#region

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace MITD.Fuel.Domain.Model.Enums
{
    public enum CorrectionPricingTypeEnum
    {
        [Description(" ")]
        NotDefined = 0,

        [Description("پیشفرض")]
        Default = 1,

        [Description("آخرین خرید قیمت دار")]
        LastPurchasePrice = 2,

        [Description("آخرین حواله مصرف")]
        LastIssuedConsumption = 3,

        [Description("قیمت دهی مستقیم")]
        ManualPricing = 4
    }
}