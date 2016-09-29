#region

using MITD.Domain.Model;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.IDomainServices;

#endregion

namespace MITD.Fuel.Domain.Model.Specifications
{
    public class OrderIsOpenState : SpecificationBase<Order>
    {
        public OrderIsOpenState() :
            base(
            ac => ac.State == States.Open
            )
        {
        }
    }
}