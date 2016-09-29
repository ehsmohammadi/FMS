using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MITD.Fuel.Presentation.Contracts.Enums
{
    public enum CorrectionTypeEnum
    {
        [Description(" ")]
        NotDefined = 0,

        [Description("افزایشی")]
        Plus = 1,

        [Description("کاهشی")]
        Minus = 2,
    }
}
