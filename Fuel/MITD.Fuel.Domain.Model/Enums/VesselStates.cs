using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MITD.Fuel.Domain.Model.Enums
{
    public enum VesselStates
    {
        Inactive = 1,
        CharterIn = 2,
        CharterOut = 3,
        Owned  = 4,
        Scrapped = 5,
    }
}
