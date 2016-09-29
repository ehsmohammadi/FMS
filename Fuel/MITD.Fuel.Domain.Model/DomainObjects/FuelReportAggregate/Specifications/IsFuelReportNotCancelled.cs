using MITD.Domain.Model;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.Enums;

namespace MITD.Fuel.Domain.Model.Specifications
{
    public class IsFuelReportNotCancelled : SpecificationBase<FuelReport>
    {
        public IsFuelReportNotCancelled()
            : base(
                fr =>
                    //(fr.Voyage == null || fr.Voyage.IsActive) &&  //The validity of voyage will be checked during system operations such as submit.
                    fr.State != States.Cancelled)
        {

        }
    }
}
