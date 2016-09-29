#region

using MITD.Domain.Model;
using MITD.Fuel.Domain.Model.DomainObjects;

#endregion

namespace MITD.Fuel.Domain.Model.IDomainServices
{
    public interface IOrderItemDomainService : IDomainService
    {
        void DeleteOrderItem(OrderItem orderItem);


    }
}