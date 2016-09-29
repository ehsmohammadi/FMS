using MITD.Domain.Model;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.Enums;

namespace MITD.Fuel.Domain.Model.Specifications
{
    public class OrderIsSubmitRejectedState : SpecificationBase<Order>
    {
        public OrderIsSubmitRejectedState() :
            base(
            ac => ac.State == States.SubmitRejected
            )
        {
        }
    }
}