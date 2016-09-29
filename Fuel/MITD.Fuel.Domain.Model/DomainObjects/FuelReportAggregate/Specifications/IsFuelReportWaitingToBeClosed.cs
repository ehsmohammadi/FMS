using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Domain.Model;
using MITD.Fuel.Domain.Model.DomainObjects;

namespace MITD.Fuel.Domain.Model.Specifications
{
    public class IsFuelReportWaitingToBeClosed : SpecificationBase<FuelReport>
    {
        public IsFuelReportWaitingToBeClosed()
            : base(fr => fr.State == Enums.States.Submitted)
        {

        }
    }
}
