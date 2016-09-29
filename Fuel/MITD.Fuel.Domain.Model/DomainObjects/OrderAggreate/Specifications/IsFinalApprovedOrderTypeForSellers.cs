using MITD.Domain.Model;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.Enums;

namespace MITD.Fuel.Domain.Model.Specifications
{
    public class IsFinalApprovedOrderTypeForSuppliers : SpecificationBase<Order>
    {
        public IsFinalApprovedOrderTypeForSuppliers(OrderTypes orderType, long companyId) :
            base(
            new OrderIsSubmitedState().Predicate.And
                (c => c.SupplierId == companyId && c.OrderType == orderType)
            )
        {
        }
    }
}