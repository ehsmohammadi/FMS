using MITD.Fuel.Domain.Model.DomainObjects;

namespace MITD.Fuel.Domain.Model.IDomainServices
{
    public interface IApprovableOrderDomainService : IApprovableDomainService
    {
        void SubmiteOrder(Order order);

        void CloseOrder(Order order);

        void CancelOrder(Order order);

    }
}