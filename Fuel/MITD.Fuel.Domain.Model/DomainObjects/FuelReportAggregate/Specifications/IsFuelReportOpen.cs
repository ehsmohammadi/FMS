#region

using MITD.Domain.Model;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.IDomainServices;

#endregion

namespace MITD.Fuel.Domain.Model.Specifications
{
    public class IsFuelReportOpen : SpecificationBase<FuelReport>
    {
        public IsFuelReportOpen() :
            base(
            fr => fr.State == States.Open || fr.State==States.SubmitRejected
            )
        {
        }
    }
}