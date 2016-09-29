using MITD.Domain.Model;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.Enums;

namespace MITD.Fuel.Domain.Model.Specifications
{
    public class IsFuelReportClosed : SpecificationBase<FuelReport>
    {
        public IsFuelReportClosed()
            : base(
            fr => fr.State == States.Closed || fr.State == States.Submitted
            )
        {
        }
    }
}