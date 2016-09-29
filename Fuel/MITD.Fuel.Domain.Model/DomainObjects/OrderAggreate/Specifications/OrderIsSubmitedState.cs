using MITD.Domain.Model;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.Enums;

namespace MITD.Fuel.Domain.Model.Specifications
{
    public class OrderIsSubmitedState : SpecificationBase<Order>
    {
        public OrderIsSubmitedState() :
            base(
            ac => ac.State == States.Submitted
            )
        {
        }
    }
}