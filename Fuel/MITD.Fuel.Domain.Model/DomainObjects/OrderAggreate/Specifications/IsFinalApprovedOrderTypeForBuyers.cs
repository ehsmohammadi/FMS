using MITD.Domain.Model;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.Enums;

namespace MITD.Fuel.Domain.Model.Specifications
{
    public class IsFinalApprovedOrderTypeForOwners : SpecificationBase<Order>
    {
        public IsFinalApprovedOrderTypeForOwners(OrderTypes orderType, long companyId) :
            base(
            new OrderIsSubmitedState().Predicate.And
                (c => c.OwnerId == companyId && c.OrderType == orderType)
            )
        {
        }
    }
}