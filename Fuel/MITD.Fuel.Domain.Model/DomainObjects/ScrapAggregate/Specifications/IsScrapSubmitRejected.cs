#region

using MITD.Domain.Model;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.IDomainServices;

#endregion

namespace MITD.Fuel.Domain.Model.Specifications
{
    public class IsScrapSubmitRejected : SpecificationBase<Scrap>
    {
        public IsScrapSubmitRejected() :
            base(
            s => s.State == States.SubmitRejected
            )
        {
        }
    }
}