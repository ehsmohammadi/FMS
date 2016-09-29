using MITD.Domain.Model;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.Enums;

namespace MITD.Fuel.Domain.Model.Specifications
{
    public class IsFuelReportSubmitted : SpecificationBase<FuelReport>
    {
        public IsFuelReportSubmitted()
            : base(fr => fr.State == States.Submitted)
        {

        }
    }
}
