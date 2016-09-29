using MITD.Domain.Model;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.Enums;

namespace MITD.Fuel.Domain.Model.Specifications
{
    public class IsFuelReportSubmitRejected : SpecificationBase<FuelReport>
    {
        public IsFuelReportSubmitRejected()
            : base(fr => fr.State == States.SubmitRejected)
        {

        }
    }
}
